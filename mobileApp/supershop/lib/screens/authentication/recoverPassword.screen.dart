import 'package:flutter/material.dart';
import 'package:supershop/screens/authentication/login.screen.dart';

class RecoverPasswordScreen extends StatefulWidget {
  RecoverPasswordScreen({Key key}) : super(key: key);
  static String routeName = "/recoverPasswordScreen";
  @override
  State<RecoverPasswordScreen> createState() => _RecoverPasswordScreenState();
}

class _RecoverPasswordScreenState extends State<RecoverPasswordScreen> {
  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    return Scaffold(
      body: SafeArea(child: ListView(
        children: [
          Padding(
            padding: EdgeInsets.only(left: screenSize.width*0.1,right: screenSize.width*0.1,top: screenSize.height*0.6),
            child: ElevatedButton(
              onPressed: (){
                Navigator.pushNamed(context, LoginScreen.routeName);
              },
              child: Text(
                "Regresar al Login"
              ),
            ),
          )
        ],
      ),
    ));
  }
}