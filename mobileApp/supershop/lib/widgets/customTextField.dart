import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';

class CustomTextField extends StatelessWidget {

  const CustomTextField({this.svgColor,this.foreColor,this.bgColor, @required this.controlador,@required this.useIcon,this.svgRoute,this.label,Key key}) : super(key: key);
  final TextEditingController controlador;
  final String label;
  final String svgRoute;
  final bool useIcon;
  final Color bgColor;
  final Color svgColor;
  final Color foreColor;
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: TextField(
        style: TextStyle(
          // fontSize: 18
          // height: 2,
        ),
                  controller: controlador,                
                  decoration: 
                  InputDecoration(                    
                    border: OutlineInputBorder(borderSide: BorderSide.none,borderRadius: BorderRadius.all(Radius.circular(30))),
                    // labelText: label,
                    hintText: label,
                    isDense: true,                     
                    prefixIcon: useIcon?Container(
                      padding: EdgeInsets.only(left: 20,right: 20),
                      child: SvgPicture.asset(svgRoute,color:svgColor,height: 30,),
                    ):SizedBox() ,  
                    filled:true,
                    fillColor: bgColor,
                    labelStyle: TextStyle(
                      fontSize: 20,
                    color: foreColor
                  ),     
                  ),
                ),
    );
  }
}