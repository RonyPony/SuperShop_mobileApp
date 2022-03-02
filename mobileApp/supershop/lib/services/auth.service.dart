import 'package:cool_alert/cool_alert.dart';
import 'package:dio/dio.dart';
import 'package:get/get.dart' as prefix ;
import 'package:shared_preferences/shared_preferences.dart';
import 'package:supershop/contracts/auth_service.contract.dart';
import 'package:supershop/helpers/requestsManager.dart';
import 'package:supershop/models/loginResponse.model.dart';
import 'package:supershop/models/userInfo.model.dart';
import 'package:supershop/models/userCredentials.model.dart';
import 'package:supershop/models/userToRegisterInfo.model.dart';

class AuthenticationService implements AuthServiceContract {
  @override
  Future<LoginResponse> login(UserCredentials credentialsInfo) async {
    final sharedPreferenses = await SharedPreferences.getInstance();
    if (credentialsInfo.isGuest) {
      //TODO login Guest User
    } else {
      return loginNotGuestUser(sharedPreferenses, credentialsInfo);
    }
  }

  Future<LoginResponse> loginNotGuestUser(SharedPreferences sharedPreferenses,
      UserCredentials credentialsInfo) async {
    String result = "";
    try {
      final client = RequestsManager.requester();
      final queryParam = {
        'userName': credentialsInfo.email,
        'password': credentialsInfo.password
      };
      final response = await client.post("/auth/UserAuth/login",
          data: queryParam);
      if (response.statusCode < 400) {
        //TODO
      } else {
        //TODO
      }
    } catch (e) {
      throw e;
    }
  }

  @override
  Future<UserInfo> registerUser(UserToRegisterInfo userInfo) async {
  try {
    
      userInfo.userName = "tmpUsername";

      Dio cliente = RequestsManager.createRequester();
      Response resp = await cliente.post("auth/UserAuth/register", data: {
        'name': userInfo.name,
        'lastName': userInfo.lastName,
        'email': userInfo.email,
        'userName': userInfo.userName,
        'password': userInfo.password
      });
      print(resp);
  } catch (e) {
    throw e;
  }
   
  }
}
