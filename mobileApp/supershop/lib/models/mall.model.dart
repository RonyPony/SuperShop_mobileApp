import 'package:supershop/models/branch.model.dart';

class Mall {
  String name;
  Coordinates coordinates;
  String imageUrl;
  List<Branch> branches;
  String id;
  String createdAt;
  String updatedAt;

  Mall(
      {this.name,
      this.coordinates,
      this.imageUrl,
      this.branches,
      this.id,
      this.createdAt,
      this.updatedAt});

  Mall.fromJson(Map<String, dynamic> json) {
    name = json['name'];
    coordinates = json['coordinates'] != null
        ? new Coordinates.fromJson(json['coordinates'])
        : null;
    imageUrl = json['imageUrl'];
    if (json['branches'] != null) {
      branches = <Branch>[];
      json['branches'].forEach((v) {
        branches.add(new Branch.fromJson(v));
      });
    }
    id = json['id'];
    createdAt = json['createdAt'];
    updatedAt = json['updatedAt'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['name'] = this.name;
    if (this.coordinates != null) {
      data['coordinates'] = this.coordinates.toJson();
    }
    data['imageUrl'] = this.imageUrl;
    if (this.branches != null) {
      data['branches'] = this.branches.map((v) => v.toJson()).toList();
    }
    data['id'] = this.id;
    data['createdAt'] = this.createdAt;
    data['updatedAt'] = this.updatedAt;
    return data;
  }
}

class Coordinates {
  double lat;
  double long;

  Coordinates({this.lat, this.long});

  Coordinates.fromJson(Map<String, dynamic> json) {
    lat = json['lat'];
    long = json['long'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['lat'] = this.lat;
    data['long'] = this.long;
    return data;
  }
}