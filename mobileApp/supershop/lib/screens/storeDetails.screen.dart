import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:supershop/models/branch.model.dart';
import 'package:supershop/models/mall.model.dart';
import 'package:supershop/models/product.model.dart';
import 'package:supershop/providers/productProvider.dart';
import 'package:supershop/screens/productDetails.screen.dart';
import 'package:supershop/screens/stores.dart';
import 'package:supershop/widgets/customTextField.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';
import 'package:supershop/widgets/storeLogo.dart';

import 'cart.screen.dart';

class StoreDetailsScreen extends StatefulWidget {
  StoreDetailsScreen({Key key}) : super(key: key);
  static String routeName = "/storedetailsscreen";
  @override
  State<StoreDetailsScreen> createState() => _StoreDetailsScreenState();
}

class _StoreDetailsScreenState extends State<StoreDetailsScreen> {
  TextEditingController searchTxt;
  int prodIdCounter = 0;
  String _testProductName = 'Pantalon Jean';
  double _testProductPrice = 800;
  ScrollController _cont = ScrollController();
  String _testProductImage =
      'https://shop.loisjeans.com/13500-thickbox_default/jeans-coty-pantalon-denim-rotos.jpg';
  String storeImageUrl =
      "https://socialistmodernism.com/wp-content/uploads/2017/07/placeholder-image.png?w=640";
  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context).settings.arguments as Branch;
    storeImageUrl = args.imageUrl;
    final _productProvider =
        Provider.of<ProductProvider>(context, listen: false);
    Future<List<Product>> _productos =
        _productProvider.getProductsFromStore(args);

    return Scaffold(
      // drawer: SideMenuDrawer(),
      appBar: AppBar(
        actions: [
          GestureDetector(
              onTap: () {
                Navigator.pushNamed(context, CartScreen.routeName);
              },
              child: Icon(Icons.shopping_cart_rounded))
        ],
        title: Text(args.name),
      ),
      body: SingleChildScrollView(
        // controller: controller,
        child: Column(
          children: [
            _buildStoreLogo(),
            _buildSearchBar(),
            FutureBuilder<List<Product>>(
              future: _productos,
              builder: (context, snapshot) {
                if (snapshot.connectionState == ConnectionState.done) {
                  if (snapshot.hasError) {
                    return Text("Error, please check your connection");
                  }

                  if (snapshot.hasData) {
                    Size screenSize = MediaQuery.of(context).size;

                    return Container(
                      height: screenSize.height * 0.5,
                      child: Scrollbar(
                        isAlwaysShown: true,
                        radius: Radius.circular(50),
                        thickness: 15,
                        controller: _cont,
                        child: ListView.builder(
                          controller: _cont,
                          shrinkWrap: true,
                          itemCount: snapshot.data.length,
                          itemBuilder: (context, index) {
                            return _buildProductIcon(snapshot.data[index]);//Text(snapshot.data[index].name);
                          },
                        ),
                      ),
                    );
                  }
                } else {
                  return Center(
                    child: CircularProgressIndicator(),
                  );
                }
              },
            ),
            // Row(
            //   children: [
            //     _buildProductIcon(),
            //     _buildProductIcon(),
            //     _buildProductIcon(),
            //     _buildProductIcon()
            //   ],
            // ),
            // Row(
            //   children: [
            //     _buildProductIcon(),
            //     _buildProductIcon(),
            //     _buildProductIcon(),
            //     _buildProductIcon()
            //   ],
            // ),
            // Row(
            //   children: [
            //     _buildProductIcon(),
            //     _buildProductIcon(),
            //     _buildProductIcon(),
            //     _buildProductIcon()
            //   ],
            // ),
            // Row(
            //   children: [
            //     _buildProductIcon(),
            //     _buildProductIcon(),
            //     _buildProductIcon(),
            //     _buildProductIcon()
            //   ],
            // )
          ],
        ),
      ),
    );
  }

  _buildProductIcon(Product producto) {
    return Padding(
        padding: const EdgeInsets.only(top: 15, bottom: 15, left: 5, right: 5),
        child: Column(
          children: [
            GestureDetector(
              onTap: () {                
                Navigator.pushNamed(context, ProductDetailsScreen.routeName,
                    arguments: producto);
              },
              child: Container(
                height: 150,
                width: 200,
                child: Image.network(producto.imageUrl,),
                decoration: BoxDecoration(
                    color: Colors.blue,
                    shape: BoxShape.rectangle,
                    borderRadius: BorderRadius.only(
                        topRight: Radius.circular(15.0),
                        bottomLeft: Radius.circular(15.0),
                        topLeft: Radius.circular(15.0),
                        bottomRight: Radius.circular(15.0))),
              ),
            ),
            Text(producto.name),
            Text(
              producto.price.toString(),
              style: TextStyle(fontSize: 20),
            )
          ],
        ));
  }

  _buildStoreLogo() {
    return Container(height: 90, child: Image.network(storeImageUrl));
  }

  _buildSearchBar() {
    return CustomTextField(
      controlador: searchTxt,
      useIcon: true,
      svgRoute: "assets/lupa-icon.svg",
      bgColor: Colors.white,
      label: "Buscar",
    );
  }
}
