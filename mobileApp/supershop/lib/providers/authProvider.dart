import 'package:flutter/cupertino.dart';
import 'package:supershop/contracts/auth_service.contract.dart';
import 'package:supershop/models/userInfo.model.dart';
import 'package:supershop/models/userToRegisterInfo.model.dart';
import 'package:supershop/services/auth.service.dart';

class AuthProvider with ChangeNotifier {
  AuthServiceContract _authContract;

  AuthProvider(this._authContract);
  
  Future<UserInfo> registerUser(UserToRegisterInfo userToRegister) async {
    try {
      final response = await _authContract.registerUser(userToRegister);
      return response;
    } catch (e) {
      throw e;
    }
  }
}
