import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/branch.model.dart';
import 'package:supershop/models/mall.model.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/screens/storeDetails.screen.dart';
import 'package:supershop/widgets/customTextField.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

import 'cart.screen.dart';

class StoresScreen extends StatefulWidget {
  StoresScreen({Key key}) : super(key: key);
  static String routeName = "/storesScreen";
  @override
  State<StoresScreen> createState() => _StoresScreenState();
}

class _StoresScreenState extends State<StoresScreen> {
  Mall currentMall;

  ScrollController _scrollController = ScrollController();
  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context).settings.arguments as Mall;
    currentMall = args;
    final _prodProvider = Provider.of<ProductProvider>(context, listen: false);
    Future<List<Branch>> _futuro = _prodProvider.getMallStores(currentMall);
    TextEditingController searchTxt;
    Size screenSize = MediaQuery.of(context).size;
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
        title: Text("Tiendas en " + args.name),
        actions: [
          GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, CartScreen.routeName);
              },
              child: Icon(Icons.shopping_cart_rounded))
        ],
        backgroundColor: Colors.blue,
      ),
      body: SingleChildScrollView(
        // controller: controller,
        child: Column(
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
                        Image.network(
                          args.imageUrl,
                          width: screenSize.height * 0.3,
                        ),
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
                  "assets/filtro-icon.svg",
                  height: 20,
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
              useIcon: true,
              svgRoute: "assets/lupa-icon.svg",
              bgColor: Colors.white,
              label: "Buscar",
            ),
            SizedBox(
              height: screenSize.height * 0.05,
            ),
            FutureBuilder<List<Branch>>(
              future: _futuro,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.done) {
                  if (snapshot.hasError) {
                    return Text('Please check your connection');
                  }
                  if (snapshot.hasData) {
                    return Container(
                      height: screenSize.height * 0.5,
                      child: Scrollbar(
                        isAlwaysShown: true,
                        radius: Radius.circular(50),
                        thickness: 15,
                        controller: _scrollController,
                        child: ListView.builder(
                          controller: _scrollController,
                          shrinkWrap: true,
                          itemCount: snapshot.data.length,
                          itemBuilder: (context, index) {
                            return _buildSubtitle(snapshot.data[index]);
                          },
                        ),
                      ),
                    );
                  }
                } else {
                  return CircularProgressIndicator();
                }
              },
            ),
            // _buildTitle("Vestimenta"),
            // SizedBox(
            //   height: 15,
            // ),
            // _buildSubtitle("SportLine America"),
            // SizedBox(
            //   height: 15,
            // ),
            // _buildSubtitle("MegaModa"),
            // SizedBox(
            //   height: 15,
            // ),
            // _buildSubtitle("Anthony's"),
            // SizedBox(
            //   height: screenSize.height * 0.05,
            // ),
            // _buildTitle("Comida"),
            // SizedBox(
            //   height: 15,
            // ),
            // _buildSubtitle("Pollos Victorina"),
            // SizedBox(
            //   height: 15,
            // ),
            // _buildSubtitle("Burger King"),
            // SizedBox(
            //   height: 15,
            // ),
            // _buildSubtitle("Jade Teriyaki"),
          ],
        ),
      ),
    );
  }

  _buildTitle(String title) {
    Size screenSize = MediaQuery.of(context).size;
    return Container(
        padding: EdgeInsets.only(
            top: screenSize.height * 0.01,
            bottom: screenSize.height * 0.01,
            right: screenSize.width * 0.3,
            left: screenSize.width * 0.3),
        decoration: BoxDecoration(
            color: Colors.blue, borderRadius: BorderRadius.circular(50)),
        child: Center(
          child: Text(
            title,
            style: TextStyle(color: Colors.white, fontSize: 28),
          ),
        ));
  }

  _buildSubtitle(Branch tienda) {
    Size screenSize = MediaQuery.of(context).size;
    return GestureDetector(
      onTap: () {
        Navigator.pushNamed(context, StoreDetailsScreen.routeName,
            arguments: tienda);
      },
      child: Container(
          padding: EdgeInsets.only(
              top: screenSize.height * 0.01,
              bottom: screenSize.height * 0.01,
              right: screenSize.width * 0.20,
              left: screenSize.width * 0.20 /*  */),
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
          child: Center(
            child: Text(
              tienda.name,
              style: TextStyle(color: Colors.black, fontSize: 20),
            ),
          )),
    );
  }
}
