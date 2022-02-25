import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:supershop/constants.dart';
import 'package:supershop/screens/stores.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';
import 'package:supershop/widgets/storeLogo.dart';

class HomeScreen extends StatefulWidget {
  HomeScreen({Key key}) : super(key: key);
  static String routeName = "/homeScreen";

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    return Scaffold(
      drawer: SideMenuDrawer(),
      appBar: AppBar(
        backgroundColor: Colors.blue,
        actions: [
          Icon(Icons.shopping_cart_rounded)
        ],
      ),
      body: Column(
        children: [
         Padding(
           padding: EdgeInsets.only(top:screenSize.height*0.06 ),
           child: Row(
           mainAxisAlignment: MainAxisAlignment.center,
           children: [
              Text("Donde quieres comprar ?",style: TextStyle(
                fontSize: 18,
                color: Colors.blue,
                fontWeight: FontWeight.bold
              ),)
           ],
         ),
         ),
         SizedBox(height: 20,),
         StoreLogo(
           onTap: (){
             Navigator.pushNamed(context, StoresScreen.routeName);
            //  Navigator.pushAndRemoveUntil(
            //           context,   
            //           MaterialPageRoute(builder: (BuildContext context) => StoresScreen()), 
            //           ModalRoute.withName(StoresScreen.routeName)
            //       );
           },
           storeName: "Megacentro",
           storeLogo: "assets/logos/mega.png",
         ),
         SizedBox(height: 20,),
         StoreLogo(
           storeName: "Sambil",
           storeLogo: "assets/logos/sambil.png",
         ),
        ],
      )
    );
  }
}
