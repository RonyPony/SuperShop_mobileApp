import 'package:flutter/cupertino.dart';
import 'package:get/get.dart';
import 'package:supershop/contracts/auth_service.contract.dart';
import 'package:supershop/models/loginResponse.model.dart';
import 'package:supershop/models/userCredentials.model.dart';
import 'package:supershop/models/userInfo.model.dart';
import 'package:supershop/models/userToRegisterInfo.model.dart';
import 'package:supershop/screens/home.screen.dart';
import 'package:supershop/services/auth.service.dart';

class AuthProvider with ChangeNotifier {
  AuthServiceContract _authContract;

  AuthProvider(this._authContract);

  Future<LoginResponse> login(UserCredentials credentials)async{
    try {
      final response = await _authContract.login(credentials);
      return response;
    } catch (e) {
      throw e;
    }
  }

    Future<bool> logout()async{
    try {
      final response = await _authContract.logout();
      return response;
    } catch (e) {
      throw e;
    }
  }

  Future<UserInfo>getUserInfo(String email) async {
    try {
      final response = await _authContract.getUserInfo(email);
      return response;
    } catch (e) {
      return null;
    }
  }

  Future<UserInfo>getLocalActiveUser() async {
    try {
      final response = await _authContract.getLocalActiveUser();
      return response;
    } catch (e) {
      return null;
    }
  }
  
  Future<UserInfo> registerUser(UserToRegisterInfo userToRegister) async {
    try {
      final response = await _authContract.registerUser(userToRegister);
      return response;
    } catch (e) {
      throw e;
    }
  }

  canSkipLogin(BuildContext ctx) async {
        UserInfo currentUser = await getLocalActiveUser();
        if (currentUser == null) {
          return null;
        }else{
          // BuildContext ctx = Get;
          Navigator.pushNamedAndRemoveUntil(ctx, HomeScreen.routeName, (route) => false);
        }
  }
}
