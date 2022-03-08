class UserInfo {
  String id;
  String name;
  String lastName;
  String email;
  String userName;
  String password;

  UserInfo(
      {this.id,this.name, this.lastName, this.email, this.userName, this.password});

  UserInfo.fromJson(Map<String, dynamic> json) {
    id= json['id'];
    name = json['name'];
    lastName = json['lastName'];
    email = json['email'];
    userName = json['userName'];
    password = json['password'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['name'] = this.name;
    data['lastName'] = this.lastName;
    data['email'] = this.email;
    data['userName'] = this.userName;
    data['password'] = this.password;
    return data;
  }
}