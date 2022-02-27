class UserToRegisterInfo {
  String name;
  String lastName;
  String email;
  String userName;
  String password;
  String birthDate;

  UserToRegisterInfo(
      {this.name,
      this.lastName,
      this.email,
      this.userName,
      this.password,
      this.birthDate});

  UserToRegisterInfo.fromJson(Map<String, dynamic> json) {
    name = json['name'];
    lastName = json['lastName'];
    email = json['email'];
    userName = json['userName'];
    password = json['password'];
    birthDate = json['birthDate'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['name'] = this.name;
    data['lastName'] = this.lastName;
    data['email'] = this.email;
    data['userName'] = this.userName;
    data['password'] = this.password;
    data['birthDate'] = this.birthDate;
    return data;
  }
}