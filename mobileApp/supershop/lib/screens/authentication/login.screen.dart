import 'package:cool_alert/cool_alert.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:provider/provider.dart';
import 'package:supershop/constants.dart';
import 'package:supershop/models/loginResponse.model.dart';
import 'package:supershop/models/userCredentials.model.dart';
import 'package:supershop/providers/authProvider.dart';
import 'package:supershop/screens/authentication/forgottenPassword.screen.dart';
import 'package:supershop/screens/authentication/register.screen.dart';
import 'package:supershop/screens/home.screen.dart';
import 'package:supershop/widgets/customTextField.dart';

class LoginScreen extends StatelessWidget {
  const LoginScreen({Key key}) : super(key: key);
  static String routeName = "/loginScreen";
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: MyStatefulWidget(),
    );
  }
}

class MyStatefulWidget extends StatefulWidget {
  const MyStatefulWidget({Key key}) : super(key: key);

  @override
  State<MyStatefulWidget> createState() => _MyStatefulWidgetState();
}

class _MyStatefulWidgetState extends State<MyStatefulWidget> {
  TextEditingController _emailController =
      TextEditingController(text: "ronel.cruz.a8@gmail.com");
  TextEditingController _passwordController =
      TextEditingController(text: "Ronel08!");

  @override
  Widget build(BuildContext context) {
    bool _rememberMe = false;
    Size screenSize = MediaQuery.of(context).size;
    return Scaffold(
      backgroundColor: Colors.blue.shade600,
      body: Padding(
          padding: EdgeInsets.only(
              top: screenSize.height * 0.05,
              left: screenSize.width * 0.1,
              right: screenSize.width * 0.1),
          child: ListView(
            children: <Widget>[
              Container(
                  alignment: Alignment.center,
                  padding: EdgeInsets.only(
                      top: MediaQuery.of(context).size.height * 0.1),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Padding(
                        padding: EdgeInsets.symmetric(horizontal: 10.0),
                        child: Container(
                          height: 2.0,
                          width: 40.0,
                          color: Colors.white,
                        ),
                      ),
                      Text(
                        // appName,
                        "Inicio sesion",
                        style: TextStyle(
                            color: Colors.white,
                            fontWeight: FontWeight.w500,
                            fontSize: 30),
                      ),
                      Padding(
                        padding: EdgeInsets.symmetric(
                          horizontal: 10.0,
                        ),
                        child: Container(
                          height: 2.0,
                          width: 40.0,
                          color: Colors.white,
                        ),
                      ),
                    ],
                  )),
              SizedBox(
                height: 20,
              ),
              CustomTextField(
                foreColor: Colors.grey,
                bgColor: Colors.white,
                controlador: _emailController,
                useIcon: true,
                svgColor: Colors.black,
                svgRoute: "assets/user.svg",
                label: "Correo Electronico",
              ),
              CustomTextField(
                isPassword: true,
                foreColor: Colors.grey,
                bgColor: Colors.white,
                controlador: _passwordController,
                useIcon: true,
                svgRoute: "assets/candado-login.svg",
                label: "Clave",
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Checkbox(
                      value: _rememberMe,
                      activeColor: Colors.white,
                      checkColor: Colors.blue,
                      onChanged: (value) {
                        _rememberMe = value;
                        setState(() {});
                      }),
                  Text(
                    'Recordarme',
                    style: TextStyle(color: Colors.white),
                  ),
                  SizedBox(
                    width: 20,
                  ),
                  TextButton(
                    onPressed: () {
                      Navigator.pushNamed(
                          context, ForgottenPasswrodScreen.routeName);
                    },
                    child: const Text(
                      'Olvido su contrasena?',
                      style: TextStyle(color: Colors.white),
                    ),
                  ),
                ],
              ),
              Container(
                  height: 50,
                  padding: EdgeInsets.only(
                      left: screenSize.width * 0.1,
                      right: screenSize.width * 0.1),
                  child: ElevatedButton(
                    style: ButtonStyle(
                        backgroundColor:
                            MaterialStateProperty.all(Colors.black),
                        shape:
                            MaterialStateProperty.all<RoundedRectangleBorder>(
                                RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(50),
                                    side: BorderSide(color: Colors.black)))),
                    child: const Text('Continuar'),
                    onPressed: () async {
                      final _authProvider =
                          Provider.of<AuthProvider>(context, listen: false);
                      UserCredentials cred = UserCredentials();
                      cred.email = _emailController.text;
                      cred.password = _passwordController.text;
                      LoginResponse response = await _authProvider.login(cred);
                      if (response.result.isSuccess == true) {
                        Navigator.pushNamedAndRemoveUntil(
                            context, HomeScreen.routeName, (route) => false);
                      } else {
                        print(response.result.message);
                        CoolAlert.show(
                          context: context,
                          type: CoolAlertType.error,
                          text: response.result.message,
                        );
                      }
                    },
                  )),
              Row(
                children: <Widget>[
                  TextButton(
                    child: const Text(
                      'Registrate',
                      style: TextStyle(fontSize: 18, color: Colors.black),
                    ),
                    onPressed: () {
                      Navigator.pushNamed(context, RegisterScreen.routeName);
                    },
                  )
                ],
                mainAxisAlignment: MainAxisAlignment.center,
              ),
              Padding(
                padding: EdgeInsets.only(
                    top: screenSize.height * 0.2, left: screenSize.width * 0.5),
                child: TextButton(
                    style: ButtonStyle(
                        foregroundColor:
                            MaterialStateProperty.all(Colors.white)),
                    onPressed: () {
                      //TODO
                    },
                    child: GestureDetector(
                      onTap: () {
                        Navigator.pushAndRemoveUntil(
                            context,
                            MaterialPageRoute(
                                builder: (BuildContext context) =>
                                    HomeScreen()),
                            ModalRoute.withName(HomeScreen.routeName));
                      },
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: [
                          Text('Saltar'),
                          SvgPicture.asset("assets/next.svg")
                        ],
                      ),
                    )),
              )
            ],
          )),
    );
  }
}
