class Product {
  int id;
  String name;
  int price;
  String imageUrl;

  Product({this.name, this.price, this.imageUrl});

  Product.fromJson(Map<String, dynamic> json) {
    name = json['name'];
    price = json['price'];
    imageUrl = json['imageUrl'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['name'] = this.name;
    data['price'] = this.price;
    data['imageUrl'] = this.imageUrl;
    return data;
  }
}