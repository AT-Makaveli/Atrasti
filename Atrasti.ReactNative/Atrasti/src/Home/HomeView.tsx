import React from "react";
import {
    ActivityIndicator,
    Dimensions,
    FlatList, Image, ListRenderItemInfo,
    ScrollView,
    StyleSheet, Text, TouchableOpacity,
    View
} from "react-native";
import LinearGradient from "react-native-linear-gradient";
import { AtrastiLogo } from "../utils/StaticImageUrls";
import { Home_Res } from "../Api/Models/Responds/Home_Res";
import { Product_Res } from "../Api/Models/Responds/Product_Res";
import { getLogoImage, getProductImage } from "../Api/APIData";
import { getMainPage } from "../Api/HomeAPI";
import NavigationEvent, { fromNavigationEvent } from "../utils/NavigationEvent";

interface HomeState {
    homePage: Home_Res | null
}

interface Product {
    imageUrl: string,
    name: string,
    description: string
}


const windowHeight = Dimensions.get("window").height;

export default class HomeView extends React.Component<any, HomeState> {

    private _onHomeViewBeingShown: NavigationEvent | null = null;

    constructor(props: any) {
        super(props);

        this.state = {
            homePage: null
        }
    }

    componentDidMount() {
        const event = this.props.navigation.addListener('focus', this.onRequestHomePage.bind(this));
        this._onHomeViewBeingShown = fromNavigationEvent(event);
    }

    componentWillUnmount() {
        this._onHomeViewBeingShown?.remove();
    }

    onRequestHomePage() {
        getMainPage()
            .then(result => {
                this.setState({
                    homePage: result
                });
            })
            .catch(error => {
                console.log(error);
            });
    }

    render() {
        if (this.state.homePage === null) this.renderLoading();

        return this.renderHomePage();
    }

    renderLoading() {
        return (
            <View style={{
                backgroundColor: '#000000',
                flex: 1,
            }}>
                <ActivityIndicator size={"large"}/>
            </View>
        )
    }

    renderHomePage() {
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
                    {this.renderCategories()}
                </ScrollView>
            </View>
        )
    }

    renderCategories() {
        return this.state.homePage?.categories.map(value => {
            return (
                <View style={{
                    margin: 10
                }}>
                    <Text style={{
                        color: 'white',
                        fontSize: 24,
                    }}>{value.category}</Text>

                    <FlatList data={value.products} renderItem={this.renderItem.bind(this)}
                              horizontal={true}
                              showsHorizontalScrollIndicator={false}
                              keyExtractor={this._keyExtractor}/>
                </View>
            )
        });
    }

    _keyExtractor = (item: Product_Res, index: number) => index.toString();

    renderItem(info: ListRenderItemInfo<Product_Res>) {
        return (
            <TouchableOpacity activeOpacity={0.6} style={Styles.item} onPress={() => {
                this.props.navigation.navigate('OtherProfile', {
                    userId: info.item.userId.toString()
                });
            }}>
                <Image source={{uri: getProductImage(info.item.id)}} style={{
                    width: 150,
                    height: 100,
                    borderRadius: 20
                }}/>
                <Text style={{
                    color: 'white',
                }}>{info.item.title}</Text>
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
