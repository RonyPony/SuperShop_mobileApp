// We use name route
// All our routes will be available here

import 'package:flutter/cupertino.dart';
import 'package:supershop/screens/authentication/forgottenPassword.screen.dart';
import 'package:supershop/screens/authentication/login.screen.dart';
import 'package:supershop/screens/authentication/recoverPassword.screen.dart';
import 'package:supershop/screens/authentication/register.screen.dart';
import 'package:supershop/screens/cart.screen.dart';
import 'package:supershop/screens/home.screen.dart';
import 'package:supershop/screens/productDetails.screen.dart';
import 'package:supershop/screens/storeDetails.screen.dart';
import 'package:supershop/screens/stores.dart';

final Map<String, WidgetBuilder> routes = {
  LoginScreen.routeName: (context) => LoginScreen(),
  RegisterScreen.routeName: (context) => RegisterScreen(),
  ForgottenPasswrodScreen.routeName: (contex) => ForgottenPasswrodScreen(),
  RecoverPasswordScreen.routeName:(context)=>RecoverPasswordScreen(),
  HomeScreen.routeName:(context)=>HomeScreen(),
  StoresScreen.routeName:(context)=>StoresScreen(),
  StoreDetailsScreen.routeName:(context)=>StoreDetailsScreen(),
  ProductDetailsScreen.routeName:(context)=>ProductDetailsScreen(),
  CartScreen.routeName:(context)=>CartScreen(),
};
