import 'package:supershop/models/loginResponse.model.dart';
import 'package:supershop/models/userCredentials.model.dart';
import 'package:supershop/models/userInfo.model.dart';
import 'package:supershop/models/userToRegisterInfo.model.dart';

abstract class AuthServiceContract {
  Future<LoginResponse> login(UserCredentials credentialsInfo);
  Future<UserInfo>registerUser(UserToRegisterInfo userInfo);
  Future<UserInfo>getLocalActiveUser();
  Future<UserInfo>getUserInfo(String email);
Future<bool>logout();
  
  }
  
 
