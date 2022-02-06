import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class SideMenuDrawer extends StatelessWidget {
  const SideMenuDrawer({Key key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Drawer(
        child: ListView(
          padding: EdgeInsets.zero,
          children: [
            DrawerHeader(
              child: Text(
                "Header"
              ),
              decoration: BoxDecoration(
                color: Colors.blue
              ),
            ),
            ListTile(
              leading: Icon(CupertinoIcons.house_fill,color: Colors.blue,),
              title: Text("Inicio",style: TextStyle(
                color: Colors.blue
              ),),
              onTap: (){
                //TODO
              },
            ),
            Padding(
                      padding:EdgeInsets.symmetric(horizontal:10.0),
                      child:Container(
                      height:1.0,
                      color:Colors.blue,),),
            ListTile(              
              leading: Icon(CupertinoIcons.person_fill,color: Colors.blue,),
              title: Text("Perfil",style: TextStyle(
                color: Colors.blue
              ),),
              onTap: (){
                //TODO
              },
            ),
Padding(
                      padding:EdgeInsets.symmetric(horizontal:10.0),
                      child:Container(
                      height:1.0,
                      color:Colors.blue,),),
            ListTile(
              leading: Icon(CupertinoIcons.gear_alt_fill,color: Colors.blue,),
              title: Text("Configuracion",style: TextStyle(
                color: Colors.blue
              ),),
              onTap: (){
                //TODO
              },
            ),
            Padding(
                      padding:EdgeInsets.symmetric(horizontal:10.0),
                      child:Container(
                      height:1.0,
                      color:Colors.blue,),),
          ],
        ),
      );
  }
}