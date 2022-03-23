import 'dart:convert';

import 'package:cool_alert/cool_alert.dart';
import 'package:dio/dio.dart';
import 'package:flutter/rendering.dart';
import 'package:get/get.dart' as prefix;
import 'package:shared_preferences/shared_preferences.dart';
import 'package:supershop/contracts/auth_service.contract.dart';
import 'package:supershop/helpers/requestsManager.dart';
import 'package:supershop/models/loginResponse.model.dart';
import 'package:supershop/models/userInfo.model.dart';
import 'package:supershop/models/userCredentials.model.dart';
import 'package:supershop/models/userToRegisterInfo.model.dart';

class AuthenticationService implements AuthServiceContract {
  String SAVED_ACTIVE_USER_KEY = "usuariosActivosDeSuperShop";

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
        'password': credentialsInfo.password,
        'rememberMe': credentialsInfo.remember
      };
      final response =
          await client.post("/auth/UserAuth/login", data: queryParam);
      if (response.statusCode < 400) {
        LoginResponse loginInfo = LoginResponse.fromJson(response.data);

        UserInfo info = await getUserInfo(credentialsInfo.email);
        if (await saveLocalActiveUser(info)) {
          return loginInfo;
        } else {
          return loginInfo;
        }
      } else {
        //TODO
      }
    } catch (e) {
      return null;
    }
  }

  @override
  Future<UserInfo> registerUser(UserToRegisterInfo userInfo) async {
    try {
      userInfo.userName = userInfo.email;

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

  Future<bool> saveLocalActiveUser(UserInfo loginInfo) async {
    try {
      SharedPreferences prefs = await SharedPreferences.getInstance();
      String data = json.encode(loginInfo);
      bool saved = await prefs.setString(SAVED_ACTIVE_USER_KEY, data);
      return saved;
    } catch (e) {
      return false;
    }
  }

    Future<bool> removeLocalActiveUser() async {
    try {
      SharedPreferences prefs = await SharedPreferences.getInstance();
      String data = json.encode(null);
      bool saved = await prefs.setString(SAVED_ACTIVE_USER_KEY, data);
      return saved;
    } catch (e) {
      return false;
    }
  }

  @override
  Future<UserInfo> getLocalActiveUser() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String jsonedUser = prefs.getString(SAVED_ACTIVE_USER_KEY);
    if (jsonedUser != null) {
      UserInfo user = UserInfo.fromJson(jsonDecode(jsonedUser));
      // List<Product> itemsList =
      //     List<Product>.from(parsedListJson.map((i) => Product.fromJson(i)));
      return user;
    } else {
      return null;
    }
  }

  @override
  Future<UserInfo> getUserInfo(String email) async {
    try {
      Dio cliente = RequestsManager.createRequester();
      Response resp =
          await cliente.get("auth/UserAuth/user/$email");
      if (resp.data["isSuccess"]) {
        return UserInfo.fromJson(resp.data["data"]);
      }else{
        return null;
      }
    } catch (e) {
      print(e);
    }
  }

  @override
  Future<bool> logout() {
   return removeLocalActiveUser();
  }
}
