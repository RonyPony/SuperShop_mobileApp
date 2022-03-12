import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/branch.model.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/screens/storeDetails.screen.dart';
import 'package:supershop/screens/stores.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

import 'cart.screen.dart';

class TiendasScreen extends StatefulWidget {
  TiendasScreen({Key key}) : super(key: key);
  static String routeName = "tiendasScreen";

  @override
  State<TiendasScreen> createState() => _TiendasScreenState();
}

class _TiendasScreenState extends State<TiendasScreen> {
  @override
  Widget build(BuildContext context) {
    final _prodProvider = Provider.of<ProductProvider>(context, listen: false);
    Future<List<Branch>> _futuro = _prodProvider.getAllStores();
    ScrollController controlador = ScrollController();
    Size screenSize = MediaQuery.of(context).size;
    return Scaffold(
        drawer: SideMenuDrawer(),
        appBar: AppBar(
          title: Text('Tiendas'),
          actions: [
            GestureDetector(
                onTap: () {
                  Navigator.pushNamed(context, CartScreen.routeName);
                },
                child: Icon(Icons.shopping_cart_rounded))
          ],
        ),
        body: FutureBuilder<List<Branch>>(
          future: _futuro,
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.done) {
              if (snapshot.hasError) {
                return Center(
                    child: Text('Error,please check your connection'));
              }
              if (snapshot.hasData) {
                return Container(
                  height: screenSize.height,
                  child: Scrollbar(
                    isAlwaysShown: true,
                    radius: Radius.circular(50),
                    thickness: 5,
                    controller: controlador,
                    child: ListView.builder(
                      controller: controlador,
                      shrinkWrap: true,
                      itemCount: snapshot.data.length,
                      itemBuilder: (context, index) {
                        return buildBranch(snapshot.data[index]);
                      },
                    ),
                  ),
                );
              }
            } else {
              return Center(child: CircularProgressIndicator());
            }
          },
        ));
  }

  Widget buildBranch(Branch tienda) {
    Size screenSize = MediaQuery.of(context).size;
    final _prodProvider = Provider.of<ProductProvider>(context, listen: false);
    Future<String> mallName =
        _prodProvider.getMallNameFromMallId(tienda.mallId);
    return Padding(
        padding: const EdgeInsets.only(top: 15, bottom: 15, left: 5, right: 5),
        child: Column(
          children: [
            GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, StoreDetailsScreen.routeName,
                    arguments: tienda);
              },
              child: Container(
                padding: EdgeInsets.only(bottom: 10),
                decoration: BoxDecoration(
                    color: Colors.blue,
                    borderRadius: BorderRadius.only(
                        topRight: Radius.circular(50),
                        topLeft: Radius.circular(50),
                        bottomLeft: Radius.circular(10),
                        bottomRight: Radius.circular(10))),
                child: Container(
                  height: screenSize.height * 0.2,
                  width: screenSize.width,
                  child: tienda.imageUrl != null
                      ? Image.network(tienda.imageUrl)
                      : SizedBox(),
                ),
              ),
            ),
            FutureBuilder(
              future: mallName,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.done) {
                  if (snapshot.hasError) {
                    return SizedBox();
                  }

                  if (snapshot.hasData) {
                    return Text(
                      tienda.name.toString() + " en "+snapshot.data,
                      style: TextStyle(fontSize: 20),
                    );
                  }
                } else {
                  return Center(
                    child: CircularProgressIndicator(),
                  );
                }
              },
            ),
          ],
        ));
  }
}
