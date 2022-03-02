class LoginResponse {
  Result result;
  String token;
  String expiration;

  LoginResponse({this.result, this.token, this.expiration});

  LoginResponse.fromJson(Map<String, dynamic> json) {
    result =
        json['result'] != null ? new Result.fromJson(json['result']) : null;
    token = json['token'];
    expiration = json['expiration'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    if (this.result != null) {
      data['result'] = this.result.toJson();
    }
    data['token'] = this.token;
    data['expiration'] = this.expiration;
    return data;
  }
}

class Result {
  bool isSuccess;
  String message;
  dynamic exception;

  Result({this.isSuccess, this.message, this.exception});

  Result.fromJson(Map<String, dynamic> json) {
    isSuccess = json['isSuccess'];
    message = json['message'];
    exception = json['exception'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['isSuccess'] = this.isSuccess;
    data['message'] = this.message;
    data['exception'] = this.exception;
    return data;
  }
}