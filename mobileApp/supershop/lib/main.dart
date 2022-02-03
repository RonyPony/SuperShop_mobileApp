import 'package:flutter/material.dart';
import 'package:supershop/constants.dart';
import 'package:supershop/routes.dart';
import 'package:supershop/screens/authentication/login.screen.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      routes: routes,
      theme: ThemeData(        
        primarySwatch:kPrimaryColor,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: LoginScreen(),
    );
  }
}
