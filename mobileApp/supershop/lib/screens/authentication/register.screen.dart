import 'package:cool_alert/cool_alert.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/userInfo.model.dart';
import 'package:supershop/models/userToRegisterInfo.model.dart';
import 'package:supershop/providers/authProvider.dart';
import 'package:supershop/widgets/customTextField.dart';

import '../../constants.dart';

class RegisterScreen extends StatefulWidget {
  RegisterScreen({Key key}) : super(key: key);
  static String routeName = "/registerScreen";
  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        elevation: 0,
        toolbarHeight: 80,
        backgroundColor: Colors.blue.shade600,
        title: Text("Volver"),
      ),
      body: RegisterWidget(),
    );
  }
}

class RegisterWidget extends StatefulWidget {
  const RegisterWidget({Key key}) : super(key: key);

  @override
  State<RegisterWidget> createState() => _RegisterWidgetState();
}

class _RegisterWidgetState extends State<RegisterWidget> {
  TextEditingController _nameController = TextEditingController(text: "ronel");
  TextEditingController _passwordController =
      TextEditingController(text: "Ronel08!");
  TextEditingController _confirmPasswordController =
      TextEditingController(text: "Ronel08!");
  TextEditingController _lastName = TextEditingController(text: "Cruz");
  TextEditingController _email =
      TextEditingController(text: "ronel.cruz.a8@gmail.com");
  TextEditingController _errorMessage = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Padding(
        padding: const EdgeInsets.all(10),
        child: ListView(
          children: <Widget>[
            Container(
                alignment: Alignment.center,
                padding: EdgeInsets.only(
                    top: MediaQuery.of(context).size.height * 0.01),
                child: const Text(
                  appName,
                  style: TextStyle(
                      color: kPrimaryColor,
                      fontWeight: FontWeight.w500,
                      fontSize: 30),
                )),
            Container(
                alignment: Alignment.center,
                padding: const EdgeInsets.all(10),
                child: const Text(
                  'Registrate',
                  style: TextStyle(fontSize: 20),
                )),
            Container(
              child: Text(
                _errorMessage.text,
                style: TextStyle(color: Colors.red, fontSize: 18),
              ),
            ),
            _buildFields(),
            SizedBox(
              height: 20,
            ),
            Container(
                height: 50,
                padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
                child: ElevatedButton(
                  child: const Text('Registrar'),
                  onPressed: () {
                    registerUser();
                  },
                )),
          ],
        ));
  }

  _buildFields() {
    return Column(
      children: [
        CustomTextField(
          foreColor: Colors.white,
          bgColor: Colors.blue,
          controlador: _nameController,
          useIcon: true,
          svgColor: Colors.white,
          svgRoute: "assets/user.svg",
          label: "Nombre",
        ),
        CustomTextField(
          foreColor: Colors.white,
          bgColor: Colors.blue,
          controlador: _lastName,
          useIcon: true,
          svgColor: Colors.white,
          svgRoute: "assets/family.svg",
          label: "Apellidos",
        ),
        CustomTextField(
          foreColor: Colors.white,
          bgColor: Colors.blue,
          controlador: _email,
          useIcon: true,
          svgColor: Colors.white,
          svgRoute: "assets/email.svg",
          label: "Correo Electronico",
        ),
        CustomTextField(
          foreColor: Colors.white,
          bgColor: Colors.blue,
          controlador: _passwordController,
          useIcon: true,
          isPassword: true,
          svgColor: Colors.white,
          svgRoute: "assets/password.svg",
          label: "Clave",
        ),
        CustomTextField(
          foreColor: Colors.white,
          isPassword: true,
          bgColor: Colors.blue,
          controlador: _confirmPasswordController,
          useIcon: true,
          svgColor: Colors.white,
          svgRoute: "assets/password.svg",
          label: "Repetir Clave",
        ),
      ],
    );
  }

  void registerUser() async {
    try {
      final authProvider = Provider.of<AuthProvider>(context, listen: false);
      UserToRegisterInfo tmpUsr = UserToRegisterInfo();
      tmpUsr.email = _email.text;
      tmpUsr.name = _nameController.text;
      tmpUsr.lastName = _lastName.text;
      tmpUsr.password = _passwordController.text;
      if (validateUser(tmpUsr)) {
        UserInfo response = await authProvider.registerUser(tmpUsr);
        print(response);
      }
    } catch (e) {
      CoolAlert.show(context: context, type: CoolAlertType.error,text: e.response.data.message);
    }
  }

  bool validateUser(UserToRegisterInfo user) {
    _errorMessage.text = "";
    setState(() {});
    if (user.name != "") {
      if (user.lastName != "") {
        if (user.password != "") {
          if (user.password == _confirmPasswordController.text) {
            return true;
          } else {
            _errorMessage.text = "Las claves  no coinciden";
            setState(() {});
          }
        } else {
          _errorMessage.text = "La clave  no es valida";
          setState(() {});
        }
      } else {
        _errorMessage.text = "El apellido no es valido";
        setState(() {});
      }
    } else {
      _errorMessage.text = "El nombre no es valido";
      setState(() {});
    }
  }
}
