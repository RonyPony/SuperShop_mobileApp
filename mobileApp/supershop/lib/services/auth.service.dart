import 'package:shared_preferences/shared_preferences.dart';
import 'package:supershop/contracts/auth_service.contract.dart';
import 'package:supershop/helpers/requestsManager.dart';
import 'package:supershop/models/userInfo.model.dart';
import 'package:supershop/models/userCredentials.model.dart';

class AuthenticationService implements AuthServiceContract {
  @override
  Future<UserInfo> login(UserCredentials credentialsInfo) async {
    final sharedPreferenses = await SharedPreferences.getInstance();
    if (credentialsInfo.isGuest) {
      //TODO login Guest User
    } else {
      return loginNotGuestUser(sharedPreferenses, credentialsInfo);
    }
  }

  Future<UserInfo> loginNotGuestUser(SharedPreferences sharedPreferenses,
      UserCredentials credentialsInfo) async {
    String result = "";
    try {
      final client = RequestsManager.requester();
      final queryParam = {
        'email': credentialsInfo.email,
        'password': credentialsInfo.password
      };
      final token = await client.get("token", queryParameters: queryParam);
      if (token.statusCode < 400) {
        //TODO
      } else {
        //TODO
      }
    } catch (e) {}
  }

  @override
  Future<UserInfo> registerUser(userInfo) {
    // TODO: implement registerUser
    throw UnimplementedError();
  }
}
