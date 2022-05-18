import React from "react";
import {
    Dimensions,
    FlatList, Image, ListRenderItemInfo,
    ScrollView,
    StyleSheet, Text, TouchableOpacity,
    View
} from "react-native";
import LinearGradient from "react-native-linear-gradient";
import { AtrastiLogo } from "../utils/StaticImageUrls";

interface HomeState {
    healthCareProducts: Array<Product>
}

interface Product {
    imageUrl: string,
    name: string,
    description: string
}


const windowHeight = Dimensions.get("window").height;

export default class HomeView extends React.Component<any, HomeState> {

    constructor(props: any) {
        super(props);

        this.state = {
            healthCareProducts: [
                {
                    imageUrl: "http://192.168.3.20:5000/products/11.png",
                    name: "Nike shoes",
                    description: "Testing hahahaha"
                },
                {
                    imageUrl: "http://192.168.3.20:5000/products/12.png",
                    name: "Nike shoes",
                    description: "Testing hahahaha"
                },
                {
                    imageUrl: "http://192.168.3.20:5000/products/13.png",
                    name: "Nike shoes",
                    description: "Testing hahahaha"
                },
                {
                    imageUrl: "http://192.168.3.20:5000/products/14.png",
                    name: "Nike shoes",
                    description: "Testing hahahaha"
                },
            ]
        }
    }

    render() {
        return (
            <View style={Styles.mainView}>
                <ScrollView>
                    <View style={{
                        height: windowHeight / 3,
                        backgroundColor: 'red'
                    }}>
                        <Image source={require('../../assets/images/engineblock.jpg')} style={{
                            width: '100%',
                            height: windowHeight / 3
                        }}/>
                        <LinearGradient colors={['transparent', '#000000']} start={{x: 0, y: 0.2}} end={{x: 0, y: 1}}
                                        style={{
                                            flex: 1,
                                            position: "absolute",
                                            zIndex: 12,
                                            top: 0,
                                            bottom: 0,
                                            right: 0,
                                            left: 0
                                        }}>
                            <Image source={AtrastiLogo} style={{
                                width: 118,
                                height: 27,
                                margin: 10
                            }}/>
                        </LinearGradient>
                    </View>
                    <View style={{
                        margin: 10
                    }}>
                        <Text style={{
                            color: 'white',
                            fontSize: 24,
                        }}>Health care</Text>

                        <FlatList data={this.state.healthCareProducts} renderItem={this.renderItem}
                                  horizontal={true}
                                  showsHorizontalScrollIndicator={false}
                                  keyExtractor={this._keyExtractor}/>
                    </View>
                    <View style={{
                        margin: 10
                    }}>
                        <Text style={{
                            color: 'white',
                            fontSize: 24,
                        }}>Electronics</Text>

                        <FlatList data={this.state.healthCareProducts} renderItem={this.renderItem}
                                  horizontal={true}
                                  showsHorizontalScrollIndicator={false}
                                  keyExtractor={this._keyExtractor}/>
                    </View>
                    <View style={{
                        margin: 10
                    }}>
                        <Text style={{
                            color: 'white',
                            fontSize: 24,
                        }}>Entertainment</Text>

                        <FlatList data={this.state.healthCareProducts} renderItem={this.renderItem}
                                  horizontal={true}
                                  showsHorizontalScrollIndicator={false}
                                  keyExtractor={this._keyExtractor}/>
                    </View>
                    <View style={{
                        margin: 10
                    }}>
                        <Text style={{
                            color: 'white',
                            fontSize: 24,
                        }}>Services</Text>

                        <FlatList data={this.state.healthCareProducts} renderItem={this.renderItem}
                                  horizontal={true}
                                  showsHorizontalScrollIndicator={false}
                                  keyExtractor={this._keyExtractor}/>
                    </View>

                </ScrollView>
            </View>
        )
    }

    _keyExtractor = (item: Product, index: number) => index.toString();

    renderItem(info: ListRenderItemInfo<Product>) {
        return (
            <TouchableOpacity activeOpacity={0.6} style={Styles.item} onPress={() => {
                console.log('hereee!');
            }}>
                <Image source={{uri: info.item.imageUrl}} style={{
                    width: 150,
                    height: 100,
                    borderRadius: 20
                }}/>
                <Text style={{
                    color: 'white',
                }}>{info.item.name}</Text>
            </TouchableOpacity>
        );
    };
}

const Styles = StyleSheet.create({
    mainView: {
        backgroundColor: '#000000',
        display: "flex",
        minHeight: "100%",
        flexDirection: "column"
    },
    topView: {
        backgroundColor: '#343a40',
        display: "flex",
        height: "30%",
        flexDirection: "column",
        justifyContent: "flex-end"
    },
    bodyView: {
        display: "flex",
    },
    continueButton: {
        backgroundColor: 'transparent',
        marginTop: 10,
        borderRadius: 50,
        borderColor: '#29B6F6',
        borderWidth: 2,
        width: "70%",
        left: "15%"
    },
    continueButtonText: {
        textAlign: "center",
        color: "#FFFFFF",
        padding: 10
    },
    container: {
        flex: 1,
        marginVertical: 20,
    },
    item: {
        alignItems: 'center',
        justifyContent: 'center',
        flex: 1,
        margin: 3
    },
});
