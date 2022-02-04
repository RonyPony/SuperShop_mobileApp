import 'package:supershop/models/userCredentials.model.dart';
import 'package:supershop/models/userInfo.model.dart';

abstract class AuthServiceContract {
  Future<UserInfo> login(UserCredentials credentialsInfo);
}
