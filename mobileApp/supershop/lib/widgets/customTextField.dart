import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';

class CustomTextField extends StatefulWidget {
  const CustomTextField(
      {this.svgColor,
      this.foreColor,
      this.bgColor,
      @required this.controlador,
      @required this.useIcon,
      this.svgRoute,
      this.label,
      this.isPassword = false,
      Key key})
      : super(key: key);

  final TextEditingController controlador;
  final String label;
  final String svgRoute;
  final bool useIcon;
  final Color bgColor;
  final Color svgColor;
  final Color foreColor;
  final bool isPassword;

  @override
  _CustomTextFieldState createState() => _CustomTextFieldState();
}

class _CustomTextFieldState extends State<CustomTextField> {
  // Initially password is obscure
  bool _obscureText = true;

  String _password;

  // Toggles the password show status
  void _toggle() {
    setState(() {
      _obscureText = !_obscureText;
    });
  }

  @override
  Widget build(BuildContext context) {
    bool showPass = widget.isPassword;
    return Column(
      children: [
        Padding(
          padding: const EdgeInsets.all(8.0),
          child: TextField(
            obscureText: _obscureText && showPass,
            style: TextStyle(
                // fontSize: 18
                // height: 2,
                ),
            controller: widget.controlador,
            decoration: InputDecoration(
              border: OutlineInputBorder(
                  borderSide: BorderSide.none,
                  borderRadius: BorderRadius.all(Radius.circular(30))),
              // labelText: label,
              hintText: widget.label,
              isDense: true,
              prefixIcon: widget.useIcon
                  ? Container(
                      padding: EdgeInsets.only(left: 20, right: 20),
                      child: SvgPicture.asset(
                        widget.svgRoute,
                        color: widget.svgColor,
                        height: 30,
                      ),
                    )
                  : SizedBox(),
              filled: true,
              fillColor: widget.bgColor,
              labelStyle: TextStyle(fontSize: 20, color: widget.foreColor),
            ),
          ),
        ),
        widget.isPassword
            ? Row(
                mainAxisAlignment: MainAxisAlignment.end,
                children: [
                  new GestureDetector(
                      onTap: _toggle,
                      child: new Text(_obscureText ? "Mostrar" : "Ocultar"))
                ],
              )
            : SizedBox(),
      ],
    );
  }
}
