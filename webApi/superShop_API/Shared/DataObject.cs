using FastMember;
using Newtonsoft.Json;

namespace superShop_API.Shared;

 public struct DataObject
    {
        [JsonProperty("values")]
        public dynamic Data { get; }
        [JsonIgnore]
        public string RawData { get; }

        public DataObject(object data)
        {
            Data = data;
            RawData = JsonConvert.SerializeObject(data);
        }

        public DataObject(string rawData)
        {
            if (!string.IsNullOrWhiteSpace(rawData) && rawData != "N/A")
            {
                Data = JsonConvert.DeserializeObject(rawData);
                RawData = rawData;
            }
            else
            {
                Data = JsonConvert.DeserializeObject("{}");
                RawData = "{}";
            }
        }

        public void AddData(string propertyName, object value)
        {
            if (Data != null)
            {
                var data = TypeAccessor.Create(value.GetType());
                data[Data, propertyName] = value;
            }
        }

        public (bool status, string message, Exception error) TryGetValue(string propertyName, out object value)
        {
            object temp = null;
            bool fill = false;
            string route = "";
            bool arrayValidation = false;
            try
            {
                if (propertyName.Contains("."))
                {
                    var values = propertyName.Split(".");

                    foreach (var v in values)
                    {
                        route = (route == "") ? v : route + $".{v}";
                        if (temp != null)
                        {
                            if (v.Contains("["))
                            {
                                arrayValidation = true;
                                var initialPosition = v.LastIndexOf("[") + 1;
                                var longitud = v.IndexOf("]") - initialPosition;
                                var text = v.Substring(initialPosition, longitud);
                                var number = int.Parse(text);
                                var list = temp as object[];
                                var tt = list[number];
                                temp = tt;
                            }
                            else
                            {
                                temp = ObjectAccessor.Create(temp)[v];
                            }

                        }
                        if (!fill)
                            temp = ObjectAccessor.Create(Data)[v];
                        fill = true;
                    }
                    value = temp;
                    return (true, "Dato obtenido exitosamente", null);
                }
                else
                {
                    value = ObjectAccessor.Create(Data)[propertyName];
                    return (true, "Dato obtenido exitosamente", null);
                }

            }
            catch (Exception ex)
            {
                if (arrayValidation)
                {
                    value = null;
                    return (false, $"El valor solicitado en {route} no es un arreglo", ex);
                }
                value = null;
                return (false, $"Se produjo un error al tratar de obtener el dato solicitado", ex);
            }
        }

        public bool TryConvertToAnonymousType<T>(T contract, out T result) where T : class
        {
            try
            {
                var data = JsonConvert.DeserializeAnonymousType(RawData, contract);
                result = data;
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        public bool TryConvertToObjectType<T>(out T result) where T : class
        {
            try
            {
                var data = JsonConvert.DeserializeObject<T>(RawData);
                result = data;
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Data);
        }
    }