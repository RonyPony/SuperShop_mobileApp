import 'package:supershop/models/product.model.dart';

class Branch {
  String id;
  String createdAt;
  String updatedAt;
  String name;
  String categoryId;
  String imageUrl;
  String localCode;
  String mallId;
  List<Product> products;

  Branch(
      {this.id,
      this.createdAt,
      this.updatedAt,
      this.name,
      this.categoryId,
      this.imageUrl,
      this.localCode,
      this.mallId,
      this.products});

  Branch.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    imageUrl=json['imageUrl'];
    createdAt = json['createdAt'];
    categoryId=json['categoryId'];
    updatedAt = json['updatedAt'];
    name = json['name'];
    localCode = json['localCode'];
    mallId = json['mallId'];
    if (json['products'] != null) {
      products = <Product>[];
      json['products'].forEach((v) {
        products.add(new Product.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['createdAt'] = this.createdAt;
    data['updatedAt'] = this.updatedAt;
    data['imageUrl']=this.imageUrl;
    data['categoryId']=this.categoryId;
    data['name'] = this.name;
    data['localCode'] = this.localCode;
    data['mallId'] = this.mallId;
    if (this.products != null) {
      data['products'] = this.products.map((v) => v.toJson()).toList();
    }
    return data;
  }
}

