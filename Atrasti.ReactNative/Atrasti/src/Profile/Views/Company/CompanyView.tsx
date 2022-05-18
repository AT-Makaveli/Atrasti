import React from "react";
import { Image, Modal, ScrollView, StyleSheet, Text, TouchableOpacity, View } from "react-native";
import { getLogoImage, getProductImage } from "../../../Api/APIData";
import Icon from "react-native-vector-icons/Ionicons";
import AtrastiButton from "../../../Shared/AtrastiButton";
import ProfileProductsView from "../../ProfileProductsView";
import { likeInteract } from "../../../Api/ProductAPI";
import { getWindowDimensions } from "../../../utils/WindowUtils";
import { NavigationScreenProp } from "react-navigation";
import { Product_Res } from "../../../Api/Models/Responds/Product_Res";
import { User_Res } from "../../../Api/Models/Responds/User_Res";


const windowDimensions = getWindowDimensions();
const windowWidth = windowDimensions.width;
const windowHeight = windowDimensions.height;
const profileImageWidth = windowWidth / 3;
const profileImageHeight = windowWidth / 3;

interface CompanyProps {
    navigation: NavigationScreenProp<any, any>,
    products: Product_Res[],
    user: User_Res | null,
    isProfileOwner: boolean
}

interface CompanyState {
    modalVisible: boolean,
    productIndex: number,
    products: Product_Res[],
}

export default class CompanyView extends React.Component<CompanyProps, CompanyState> {

    constructor(props: CompanyProps) {
        super(props);

        this.state = {
            modalVisible: false,
            productIndex: -1,
            products: props.products
        }
    }

    render() {
        return (
            <View style={Styles.mainView}>
                {this.state.modalVisible ? (
                    <Modal
                        animationType="slide"
                        transparent={true}
                        visible={this.state.modalVisible}
                        onRequestClose={() => {
                            this.setModalVisible(!this.state.modalVisible);
                        }}
                    >
                        <View style={modalStyles.centeredView}>
                            <View style={modalStyles.modalView}>
                                <Image source={{
                                    uri: getProductImage(this.state.products[this.state.productIndex].id)
                                }} style={{
                                    width: '100%',
                                    height: (windowWidth / 260) * 200
                                }}/>
                                <View style={{
                                    flexDirection: "row",
                                    padding: 5,
                                }}>
                                    <TouchableOpacity onPress={() => {
                                        this.onLikeInteract(this.state.productIndex);
                                    }}>
                                        <Icon
                                            name={this.state.products[this.state.productIndex].isHeartPressed ? 'heart' : 'heart-outline'}
                                            style={{
                                                fontSize: 36,
                                            }}
                                            color={this.state.products[this.state.productIndex].isHeartPressed ? '#FFFFFF' : '#939393'}/>
                                    </TouchableOpacity>
                                    <TouchableOpacity onPress={() => {
                                        this.shareInteract(this.state.productIndex);
                                    }}>
                                        <Icon name={'share-outline'} style={{
                                            fontSize: 36
                                        }} color={'#939393'}/>
                                    </TouchableOpacity>
                                </View>
                                <View style={{
                                    padding: 10
                                }}>
                                    <Text
                                        style={modalStyles.modalLikeCount}>{this.state.products[this.state.productIndex].productLikes.length} Likes</Text>
                                    <Text
                                        style={modalStyles.modalTitle}>{this.state.products[this.state.productIndex].title}</Text>
                                    <Text
                                        style={modalStyles.modalText}>{this.state.products[this.state.productIndex].description}</Text>
                                </View>
                            </View>
                        </View>
                    </Modal>
                ) : <Text></Text>}

                <ScrollView style={{
                    flex: 1,
                }}>
                    <View style={Styles.topView}>
                        {this.props.isProfileOwner && (
                            <Icon name={'add-outline'} color={'#FFFFFF'} size={24} onPress={() => {
                                this.props.navigation.navigate('BottomDrawer');
                            }} style={{
                                marginLeft: 10
                            }}/>
                        )}
                        {this.props.user !== null ? (
                            <Image
                                source={{
                                    uri: getLogoImage(this.props.user?.logo as string),
                                }}
                                style={{
                                    width: profileImageWidth,
                                    height: profileImageHeight,
                                    marginLeft: "auto",
                                    marginRight: "auto",
                                    borderRadius: profileImageWidth / 2,
                                    bottom: -(profileImageWidth / 3)
                                }}/>
                        ) : (
                            <Image
                                style={{
                                    width: profileImageWidth,
                                    height: profileImageHeight,
                                    marginLeft: "auto",
                                    marginRight: "auto",
                                    borderRadius: profileImageWidth / 2,
                                    bottom: -(profileImageWidth / 3)
                                }}/>
                        )}
                    </View>

                    <View style={{
                        padding: 6,
                        marginBottom: 30
                    }}>
                        <Text style={{
                            color: '#FFFFFF',
                            fontSize: 24,
                            textAlign: "center"
                        }}>{this.props.user?.company}</Text>
                        <Text style={{
                            color: '#FFFFFF',
                            textAlign: "center",
                            fontSize: 16
                        }}>A company whose customers is the importance.</Text>
                        <AtrastiButton title={'Contact'} onClick={() => {

                        }}
                                       style={Styles.continueButton} textStyle={Styles.continueButtonText}/>
                    </View>

                    <ProfileProductsView modalChange={this.setModalVisible.bind(this)} products={this.props.products}/>
                </ScrollView>
            </View>
        );
    }

    onLikeInteract(productIndex: number) {
        const products = this.state.products;
        const product = products[productIndex];
        likeInteract(product.id).then(result => {
            product.isHeartPressed = result.isHeartPressed;
            product.productLikes = result.productLikes;

            this.setState({
                products: products
            })
        }).catch(ex => {
            console.log('Profile like: ');
            console.log(ex);
        })
    }

    shareInteract(productIndex: number) {
        console.log('open share modal!')
    }

    setModalVisible(value: boolean, index: number = -1) {
        this.setState({
            modalVisible: value,
            productIndex: index
        })
    }
}


const Styles = StyleSheet.create({
    mainView: {
        flex: 1,
        backgroundColor: '#000000'
    },
    topView: {
        backgroundColor: '#343a40',
        display: "flex",
        paddingTop: 50,
        flexDirection: "column",
        justifyContent: "flex-end",
        marginBottom: profileImageWidth / 2
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
        width: '50%',
        flexDirection: 'row'
    },
});

const modalStyles = StyleSheet.create({
    centeredView: {
        flex: 1,
        justifyContent: "center",
        alignItems: "center",
        marginTop: 22
    },
    modalView: {
        backgroundColor: "#000000",
        borderTopLeftRadius: 20,
        borderTopRightRadius: 20,
        overflow: "hidden",
        elevation: 5,
        width: '100%',
        height: (windowHeight / 5) * 4,
        marginTop: windowHeight / 5
    },
    button: {
        borderRadius: 20,
        padding: 10,
        elevation: 2
    },
    buttonOpen: {
        backgroundColor: "#F194FF",
    },
    buttonClose: {
        backgroundColor: "#2196F3",
    },
    textStyle: {
        color: "white",
        fontWeight: "bold",
        textAlign: "center"
    },
    modalLikeCount: {
        color: 'white',
        fontWeight: "bold"
    },
    modalTitle: {
        color: 'white',
        fontWeight: "bold",
        fontSize: 18
    },
    modalText: {
        color: 'white'
    }
});
