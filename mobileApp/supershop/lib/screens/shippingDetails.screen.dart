import 'package:cool_alert/cool_alert.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/Address.model.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/screens/confirmScreen.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

import 'cart.screen.dart';
import 'home.screen.dart';

class ShoppingDetailScreen extends StatefulWidget {
  ShoppingDetailScreen({Key key}) : super(key: key);
  static String routeName = '/shoppingDetailsScreen';
  @override
  State<ShoppingDetailScreen> createState() => _ShoppingDetailScreenState();
}

class _ShoppingDetailScreenState extends State<ShoppingDetailScreen> {
  bool _isPaypal=null;
  int val = -1;

  ScrollController _scrollController = ScrollController();

  String _currentSelectedAddress="";
  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    final _productProvider =
        Provider.of<ProductProvider>(context, listen: false);
    Future<List<Address>> _addresses = _productProvider.getAddresses();
    return Scaffold(
      drawer: SideMenuDrawer(),
      appBar: AppBar(
        actions: [
          GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, CartScreen.routeName);
              },
              child: Icon(Icons.shopping_cart_rounded)),
          SizedBox(
            width: 20,
          ),
          GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, HomeScreen.routeName);
              },
              child: Icon(Icons.arrow_back_ios))
        ],
      ),
      body: Padding(
        padding: EdgeInsets.only(
            top: screenSize.height * 0.08,
            left: screenSize.width * 0.05,
            right: screenSize.width * 0.05),
        child: Column(
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  'Datos de envio',
                  style: TextStyle(
                      fontSize: 22,
                      fontWeight: FontWeight.bold,
                      color: Colors.blue),
                )
              ],
            ),
            Padding(
              padding: EdgeInsets.only(right: screenSize.width * 0.6, top: 25),
              child: Text(
                'Mis lugares',
                style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Colors.black),
              ),
            ),
            GestureDetector(
              onTap: () async {
                String direccion = await getString(
                    "Registro de direccion",
                    "Por favor ingrese su direccion completa",
                    "Direccion",
                    "Siguiente");
                String alias;
                if (direccion != "") {
                  alias = await getString(
                      "Registro de direccion",
                      "Ahora ingrese un alias para esta direccion",
                      "Alias",
                      "Guardar");
                }

                if (alias != null) {
                  Address addressToSave =
                      Address(address: direccion, addressAlias: alias);
                  bool success =
                      await _productProvider.addToAddress(addressToSave);
                  if (success) {
                    CoolAlert.show(
                        context: context,
                        type: CoolAlertType.success,
                        title: "Guardado !");
                  } else {
                    CoolAlert.show(
                        context: context,
                        type: CoolAlertType.error,
                        title: "Ha ocurrido un error",
                        text:
                            "Se ha presentado un error, por favor intentelo luego");
                  }
                }
                setState(() {});
              },
              child: Container(
                height: 60,
                width: 60,
                child: CircleAvatar(
                  backgroundColor: Colors.blue,
                  child: Icon(
                    Icons.add,
                    size: 50,
                    color: Colors.white,
                  ),
                ),
              ),
            ),
            FutureBuilder<List<Address>>(
              future: _addresses,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.done) {
                  if (snapshot.hasError) {
                    return Text(
                        "Error Occurred, check your internet connection");
                  }

                  if (snapshot.hasData) {
                    return Container(
                      child: Scrollbar(
                        controller: _scrollController,
                        thickness: 5,
                        radius: Radius.circular(10),
                        child: ListView.builder(
                          controller: _scrollController,
                          itemCount: snapshot.data.length,
                          shrinkWrap: true,
                          itemBuilder: (context, index) {
                            return _buildMyAddresses(snapshot.data[index]);
                          },
                        ),
                      ),
                    );
                  }
                } else {
                  return CircularProgressIndicator();
                }
              },
            ),
            // _buildMyAddresses(),
            // _buildMyAddresses(),
            SizedBox(
              height: screenSize.height * 0.05,
            ),
            Padding(
              padding: EdgeInsets.only(right: screenSize.width * 0.6, top: 20),
              child: Text(
                'Tipo de pago',
                style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    color: Colors.black),
              ),
            ),
            ListTile(
              title: Text("Efectivo"),
              leading: Radio(
                value: 1,

                groupValue: val,
                onChanged: (value) {
                  setState(() {
                    val = value;
                     if (val==2) {
                      _isPaypal=true;
                    }
                    if(val==1){
                      _isPaypal=false;
                    }
                    print(_isPaypal);
                  });
                },
                activeColor: Colors.blue,
              ),
            ),
            ListTile(
              title: Row(
                children: [
                  SvgPicture.asset('assets/paypal.svg',height: 30,),
                ],
              ),
              leading: Radio(
                value: 2,
                groupValue: val,
                onChanged: (value) {
                  setState(() {
                    val = value;
                    if (val==2) {
                      _isPaypal=true;
                    }
                    if(val==1){
                      _isPaypal=false;
                    }
                    print(_isPaypal);
                  });
                },
                activeColor: Colors.blue,
              ),
            ),
            _currentSelectedAddress.length>=1?Text('Su pedido se enviara a '+_currentSelectedAddress):Text('Seleccione una direccion'),
            SizedBox(
              height: screenSize.height * 0.1,
            ),
            _payButton(),
            
          ],
        ),
      ),
    );
  }

  _buildMyAddresses(Address address) {
    Size screenSize = MediaQuery.of(context).size;
    bool selected = false;
    int _selectedIndex = 0;
    return ListTile(
      title: Text(address.addressAlias),
      // selected: index == _selectedIndex,
      onTap: () {
        _currentSelectedAddress=address.address;
        setState(() {
        
        });
      },
      leading: Icon(
        Icons.location_on_outlined,
        color: Colors.blue,
        size: 45,
      ),
      trailing: GestureDetector(
          onTap: () {
            delete(address);
          },
          child: Icon(
            Icons.delete_forever,
            size: 35,
            color: Colors.red,
          )),
    );
  }

  _payButton() {
    return ElevatedButton(
      style: ButtonStyle(
          backgroundColor: MaterialStateProperty.all(Colors.blue),
          shape: MaterialStateProperty.all<RoundedRectangleBorder>(
              RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(50),
                  side: BorderSide(color: Colors.black)))),
      child: Container(
        width: 100,
        child: const Text(
          'Pagar',
          textAlign: TextAlign.center,
          style: TextStyle(
              fontSize: 22, fontWeight: FontWeight.bold, color: Colors.white),
        ),
      ),
      onPressed: () async {
        if (_currentSelectedAddress.length<1) {
          CoolAlert.show(context: context,text: "Seleccione una direccion", title: "Direccion",type: CoolAlertType.warning);
        return;
        }

        if (_isPaypal==null) {
          CoolAlert.show(context: context,text: "Seleccione un metodo de pago", title: "Metodo de pago",type: CoolAlertType.warning);
          return;
        }

        Navigator.pushNamed(context, ConfirmScreen.routeName);
      },
    );
  }

  getString(String title, String text, String hint, String btnTxt) {
    return showDialog<String>(
      context: context,
      barrierDismissible: false, // user must tap button!
      builder: (BuildContext context) {
        TextEditingController contr = TextEditingController();
        return AlertDialog(
          title: Text(title),
          content: SingleChildScrollView(
            child: ListBody(
              children: <Widget>[
                Text(text),
                TextField(
                  controller: contr,
                  decoration: InputDecoration(hintText: hint),
                )
              ],
            ),
          ),
          actions: <Widget>[
            TextButton(
              child: Text(btnTxt),
              onPressed: () {
                Navigator.of(context).pop(contr.text);
              },
            ),
          ],
        );
      },
    );
  }

  void delete(Address address) {
    final _productProvider =
        Provider.of<ProductProvider>(context, listen: false);
        _productProvider.deleteAddress(address.addressAlias);
        setState(() {
          
        });
  }
}
