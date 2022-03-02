import 'package:flutter/material.dart';
import 'package:supershop/models/product.model.dart';
import 'package:supershop/screens/productDetails.screen.dart';
import 'package:supershop/widgets/customTextField.dart';
import 'package:supershop/widgets/sideMenuDrawer.dart';

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
  int _testProductPrice = 800;
  String _testProductImage = 'https://shop.loisjeans.com/13500-thickbox_default/jeans-coty-pantalon-denim-rotos.jpg';
  @override
  Widget build(BuildContext context) {
    final args = ModalRoute.of(context).settings.arguments as String;
    
    return Scaffold(
      // drawer: SideMenuDrawer(),
      appBar: AppBar(
        title: Text(args),
      ),
      body: SingleChildScrollView(
        // controller: controller,
        child: Column(
          children: [
            _buildStoreLogo(),
            _buildSearchBar(),
            Row(
              children: [
                _buildProductIcon(),
                _buildProductIcon(),
                _buildProductIcon(),
                _buildProductIcon()
              ],
            ),
            Row(
              children: [
                _buildProductIcon(),
                _buildProductIcon(),
                _buildProductIcon(),
                _buildProductIcon()
              ],
            ),
            Row(
              children: [
                _buildProductIcon(),
                _buildProductIcon(),
                _buildProductIcon(),
                _buildProductIcon()
              ],
            ),
            Row(
              children: [
                _buildProductIcon(),
                _buildProductIcon(),
                _buildProductIcon(),
                _buildProductIcon()
              ],
            )
          ],
        ),
      ),
    );
  }

  _buildProductIcon() {
    prodIdCounter++;
    return Padding(
        padding: const EdgeInsets.only(top: 15, bottom: 15, left: 5, right: 5),
        child: Column(
          children: [
            GestureDetector(
              onTap: () {
                Product producto = Product();
                producto.id = prodIdCounter;
                producto.name=_testProductName;
                producto.price = _testProductPrice;
                producto.imageUrl = _testProductImage;
                Navigator.pushNamed(context, ProductDetailsScreen.routeName,arguments: producto);
              },
                          child: Container(
                height: 80,
                width: 80,
                child: Image.network(_testProductImage),
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
            Text(_testProductName),
            Text(
              _testProductPrice.toString(),
              style: TextStyle(fontSize: 20),
            )
          ],
        ));
  }

  _buildStoreLogo() {
    return Container(
        height: 90, child: Image.asset('assets/logos/sportline.png'));
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
