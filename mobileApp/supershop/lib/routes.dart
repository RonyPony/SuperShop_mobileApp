
// We use name route
// All our routes will be available here


import 'package:flutter/cupertino.dart';
import 'package:supershop/screens/authentication/login.screen.dart';
import 'package:supershop/screens/authentication/register.screen.dart';

final Map<String, WidgetBuilder> routes = {
  LoginScreen.routeName:(context)=>LoginScreen(),
  RegisterScreen.routeName:(context)=>RegisterScreen(),
};
