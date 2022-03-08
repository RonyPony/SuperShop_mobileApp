class Branch {
  String id;
  String createdAt;
  String updatedAt;
  String name;
  String localCode;
  String mallId;
  List<Products> products;

  Branch(
      {this.id,
      this.createdAt,
      this.updatedAt,
      this.name,
      this.localCode,
      this.mallId,
      this.products});

  Branch.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    createdAt = json['createdAt'];
    updatedAt = json['updatedAt'];
    name = json['name'];
    localCode = json['localCode'];
    mallId = json['mallId'];
    if (json['products'] != null) {
      products = <Products>[];
      json['products'].forEach((v) {
        products.add(new Products.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['createdAt'] = this.createdAt;
    data['updatedAt'] = this.updatedAt;
    data['name'] = this.name;
    data['localCode'] = this.localCode;
    data['mallId'] = this.mallId;
    if (this.products != null) {
      data['products'] = this.products.map((v) => v.toJson()).toList();
    }
    return data;
  }
}

class Products {
  String id;
  String createdAt;
  String updatedAt;
  String name;
  String code;
  String description;
  int price;
  int stock;
  String imageUrl;
  String branchId;

  Products(
      {this.id,
      this.createdAt,
      this.updatedAt,
      this.name,
      this.code,
      this.description,
      this.price,
      this.stock,
      this.imageUrl,
      this.branchId});

  Products.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    createdAt = json['createdAt'];
    updatedAt = json['updatedAt'];
    name = json['name'];
    code = json['code'];
    description = json['description'];
    price = json['price'];
    stock = json['stock'];
    imageUrl = json['imageUrl'];
    branchId = json['branchId'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['createdAt'] = this.createdAt;
    data['updatedAt'] = this.updatedAt;
    data['name'] = this.name;
    data['code'] = this.code;
    data['description'] = this.description;
    data['price'] = this.price;
    data['stock'] = this.stock;
    data['imageUrl'] = this.imageUrl;
    data['branchId'] = this.branchId;
    return data;
  }
}