import 'package:supershop/models/userCredentials.model.dart';
import 'package:supershop/models/userInfo.model.dart';
import 'package:supershop/models/userToRegisterInfo.model.dart';

abstract class AuthServiceContract {
  Future<UserInfo> login(UserCredentials credentialsInfo);
  Future<UserInfo>registerUser(UserToRegisterInformation userInfo);
  }
  
 
