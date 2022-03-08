class Address {
  String addressAlias;
  String address;

  Address({this.addressAlias, this.address});

  Address.fromJson(Map<String, dynamic> json) {
    addressAlias = json['addressAlias'];
    address = json['Address'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['addressAlias'] = this.addressAlias;
    data['Address'] = this.address;
    return data;
  }
}