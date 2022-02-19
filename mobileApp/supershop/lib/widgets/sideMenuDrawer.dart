import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:supershop/screens/home.screen.dart';

class SideMenuDrawer extends StatelessWidget {
  const SideMenuDrawer({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    return Drawer(
        child: Column(
          children: [
            Container(
              height: screenSize.height*0.9,
              child: ListView(
                padding: EdgeInsets.zero,
                children: [
                  Container(
                    height: screenSize.height*0.13,
                    child: DrawerHeader(
                      // padding: EdgeInsets.only(right: screenSize.width*0.1),
                      child: Row(
                        children: [
                          Icon(
                            Icons.close,
                            color: Colors.white,
                            size: 35,
                            ),
                        ],
                      ),
                      // child: Text(
                      //   "X",
                      //   style: TextStyle(
                      //     color: Colors.white,
                      //     fontSize: 30
                      //   ),
                      // ),
                      decoration: BoxDecoration(
                        color: Colors.blue
                      ),
                    ),
                  ),
                  GestureDetector(
                    onTap: (){
                      Navigator.pushNamedAndRemoveUntil(context, HomeScreen.routeName, (route) => false);
                    },
                    child: Container(
                      height: 50,
                      child: ListTile(                    
                        leading: SvgPicture.asset("assets/casita.svg",color: Colors.blue,height: 24,),
                        // leading: Icon(CupertinoIcons.house_fill,color: Colors.blue,),
                        title: Text("Inicio",style: TextStyle(
                          color: Colors.blue
                        ),),
                        onTap: (){
                          //TODO
                        },
                      ),
                    ),
                  ),
                  Padding(
                            padding:EdgeInsets.symmetric(horizontal:10.0),
                            child:Container(
                            height:1.0,
                            color:Colors.blue,),),
                  Container(
                    height: 50,
                    child: ListTile(              
                      leading: Icon(CupertinoIcons.person_fill,color: Colors.blue,),
                      title: Text("Perfil",style: TextStyle(
                        color: Colors.blue
                      ),),
                      onTap: (){
                        //TODO
                      },
                    ),
                  ),
Padding(
                            padding:EdgeInsets.symmetric(horizontal:10.0),
                            child:Container(
                            height:1.0,
                            color:Colors.blue,),),
                  Container(
                    height: 50,
                    child: ListTile(
                      leading: Icon(CupertinoIcons.gear_alt_fill,color: Colors.blue,),
                      title: Text("Configuracion",style: TextStyle(
                        color: Colors.blue
                      ),),
                      onTap: (){
                        //TODO
                      },
                    ),
                  ),
                  Padding(
                            padding:EdgeInsets.symmetric(horizontal:10.0),
                            child:Container(
                            height:1.0,
                            color:Colors.blue,),),
                ],
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(left: 20),
              child: Row(
                children: [
                  Text('Cerrar Sesion',
                  style: TextStyle(
                    color: Colors.blue
                  ),),
                ],
              ),
            )
          ],
        ),
      );
  }
}
