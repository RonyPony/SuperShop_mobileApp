import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

class StoresScreen extends StatefulWidget {
  StoresScreen({Key key}) : super(key: key);
  static String routeName="/storesScreen";
  @override
  State<StoresScreen> createState() => _StoresScreenState();
}

class _StoresScreenState extends State<StoresScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: SideMenuDrawer(),
      appBar: AppBar(
        backgroundColor: Colors.blue,
      ),
      body: 
          Column(
            children: [
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Padding(
                    padding: EdgeInsets.only(top: 30),
                    child: Container(
                      padding: EdgeInsets.only(right: 40,left: 40),
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(40),
                      border: Border.all(width: 2,color: Colors.blue)
                    ),
                    child: Row(
                      children: [
                        Image.asset("assets/logos/mega.png",width: 50,),
                        Text("Megacentro")
                      ],
                    ),
                  ),
                  ),
                ],
              ),
             Padding(
               padding: EdgeInsets.only(top: 20,bottom: 22),
               child:  Text('Selecciona tu establecimiento',style: TextStyle(
                color: Colors.blue,
                fontSize: 18,
                fontWeight: FontWeight.bold
              ),),
             ),
             SvgPicture.asset("assets/filter.svg",height: 30,),
             Container(
               height: 10,
               child: ListView(
               children: [
                 Image.asset("assets/logos/sambil.png"),
                 
               ],
             ),
             )
            ],
          )
    );
  }
}