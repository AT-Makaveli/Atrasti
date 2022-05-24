import React from "react";
import { Image, Modal, ScrollView, StyleSheet, Text, TouchableOpacity, View } from "react-native";
import { getWindowDimensions } from "../../../utils/WindowUtils";
import { NavigationScreenProp } from "react-navigation";
import { Product_Res } from "../../../Api/Models/Responds/Product_Res";
import { User_Res } from "../../../Api/Models/Responds/User_Res";
import { getLogoImage, getProductImage } from "../../../Api/APIData";
import Icon from "react-native-vector-icons/Ionicons";
import AtrastiButton from "../../../Shared/AtrastiButton";
import ProfileProductsView from "../../ProfileProductsView";
import { likeInteract } from "../../../Api/ProductAPI";
import { AgentPage_Res } from "../../../Api/Models/Responds/AgentPage_Res";

const windowDimensions = getWindowDimensions();
const windowWidth = windowDimensions.width;
const windowHeight = windowDimensions.height;
const profileImageWidth = windowWidth / 3;
const profileImageHeight = windowWidth / 3;

interface AgentProps {
    navigation: NavigationScreenProp<any, any>,
    user: User_Res | null,
    isProfileOwner: boolean,
    agentPage: AgentPage_Res
}

interface AgentState {
    modalVisible: boolean,
}

export default class AgentView extends React.Component<AgentProps, AgentState> {

    constructor(props: AgentProps) {
        super(props);

        this.state = {
            modalVisible: false,
        }
    }

    render() {
        return (
            <View style={Styles.mainView}>
                <ScrollView style={{
                    flex: 1,
                }}>
                    <View style={Styles.topView}>
                        <Icon name={'add-outline'} color={'#FFFFFF'} size={24} onPress={() => {
                            this.props.navigation.navigate('AgentBottomDrawer');
                        }} style={{
                            marginLeft: 10
                        }}/>
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
                            <View
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
                        }}>{this.props.user?.firstName} {this.props.user?.lastName}</Text>
                        <Text style={{
                            color: '#FFFFFF',
                            textAlign: "center",
                            fontSize: 16
                        }}>{this.props.agentPage.description}</Text>
                        <AtrastiButton title={'Contact'} onClick={() => {

                        }} style={Styles.continueButton} textStyle={Styles.continueButtonText}/>

                        <View>
                            {this.props.agentPage.address !== null && <Text style={Styles.descEntry}>{this.props.agentPage.address}</Text>}
                            {this.props.agentPage.phoneNumber !== null && <Text style={Styles.descEntry}>{this.props.agentPage.phoneNumber}</Text>}
                            {this.props.agentPage.country !== null && <Text style={Styles.descEntry}>{this.props.agentPage.country}</Text>}
                            {this.props.agentPage.county !== null && <Text style={Styles.descEntry}>{this.props.agentPage.county}</Text>}
                            {this.props.agentPage.city !== null && <Text style={Styles.descEntry}>{this.props.agentPage.city}</Text>}
                            {this.props.agentPage.website !== null && <Text style={Styles.descEntry}>{this.props.agentPage.website}</Text>}
                            {this.props.agentPage.businessSector !== null && <Text style={Styles.descEntry}>{this.props.agentPage.businessSector}</Text>}
                            {this.props.agentPage.mainProducts !== null && <Text style={Styles.descEntry}>{this.props.agentPage.mainProducts}</Text>}
                            {this.props.agentPage.mainMarkets !== null && <Text style={Styles.descEntry}>{this.props.agentPage.mainMarkets}</Text>}
                            {this.props.agentPage.yearsOfExperience !== null && <Text style={Styles.descEntry}>{this.props.agentPage.yearsOfExperience}</Text>}
                            {this.props.agentPage.certificates !== null && <Text style={Styles.descEntry}>{this.props.agentPage.certificates}</Text>}
                        </View>
                    </View>
                </ScrollView>
            </View>
        );
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
    descEntry: {
        color: '#FFFFFF',
        padding: 5
    }
});
