import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:supershop/widgets/customTextField.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

class StoresScreen extends StatefulWidget {
  StoresScreen({Key key}) : super(key: key);
  static String routeName = "/storesScreen";
  @override
  State<StoresScreen> createState() => _StoresScreenState();
}

class _StoresScreenState extends State<StoresScreen> {
  @override
  Widget build(BuildContext context) {
    Size screenSize = MediaQuery.of(context).size;
    TextEditingController searchTxt;

    _showPopupMenu() async {
      final RenderBox overlay = Overlay.of(context).context.findRenderObject();

      await showMenu(
        context: context,
        position: RelativeRect.fromLTRB(0, 0, 0, 1),
        items: [
          PopupMenuItem(
            child: Text("Show Usage"),
          ),
          PopupMenuItem(
            child: Text("Delete"),
          ),
        ],
        elevation: 8.0,
      );
    }

    // showDialog(
    //     context: context,
    //     builder: (BuildContext context) => new AlertDialog(
    //           title: new Text('Warning'),
    //           content: new Text('Hi this is Flutter Alert Dialog'),
    //           actions: <Widget>[
    //             new IconButton(
    //                 icon: new Icon(Icons.close),
    //                 onPressed: () {
    //                   Navigator.pop(context);
    //                 })
    //           ],
    //         ));

    return Scaffold(
        drawer: SideMenuDrawer(),
        appBar: AppBar(
          backgroundColor: Colors.blue,
        ),
        body: Column(
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Padding(
                  padding: EdgeInsets.only(top: 30),
                  child: Container(
                    padding: EdgeInsets.only(right: 40, left: 40),
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(40),
                      // border: Border.all(width: 0, color: Colors.blue)
                    ),
                    child: Row(
                      children: [
                        Image.asset(
                          "assets/logos/mega.png",
                          width: 70,
                        ),
                        Text("Megacentro")
                      ],
                    ),
                  ),
                ),
              ],
            ),
            SizedBox(
              height: screenSize.height * 0.05,
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                SvgPicture.asset(
                  "assets/filter.svg",
                  height: 30,
                ),
                GestureDetector(
                  onTap: () {
                    _showPopupMenu();
                  },
                  child: Text(
                    'Categorias',
                    style: TextStyle(fontSize: 20),
                  ),
                )
              ],
            ),
            CustomTextField(
              controlador: searchTxt,
              useIcon: false,
              bgColor: Colors.white,
              label: "Buscar",
            ),
            SizedBox(
              height: screenSize.height * 0.05,
            ),
            Container(
                padding: EdgeInsets.only(
                    top: screenSize.height * 0.01,
                    bottom: screenSize.height * 0.01,
                    right: screenSize.width * 0.3,
                    left: screenSize.width * 0.3),
                decoration: BoxDecoration(
                    color: Colors.blue,
                    borderRadius: BorderRadius.circular(50)),
                child: Text(
                  "Vestimenta",
                  style: TextStyle(color: Colors.white, fontSize: 28),
                )),
            SizedBox(
              height: 15,
            ),
            Container(
                padding: EdgeInsets.only(
                    top: screenSize.height * 0.01,
                    bottom: screenSize.height * 0.01,
                    right: screenSize.width * 0.28,
                    left: screenSize.width * 0.28),
                decoration: BoxDecoration(
                    boxShadow: [
                      BoxShadow(
                        color: Colors.grey.withOpacity(0.5),
                        spreadRadius: 5,
                        blurRadius: 7,
                        offset: Offset(0, 3), // changes position of shadow
                      ),
                    ],
                    color: Colors.white,
                    border: Border.all(color: Colors.black, width: 0.1),
                    borderRadius: BorderRadius.circular(50)),
                child: Text(
                  "Vestimenta",
                  style: TextStyle(color: Colors.black, fontSize: 28),
                )),
          ],
        ));
  }
}
