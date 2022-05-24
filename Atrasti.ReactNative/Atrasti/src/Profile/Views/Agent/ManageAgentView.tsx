import React from "react";
import {
    ActivityIndicator,
    DeviceEventEmitter,
    Image,
    ScrollView,
    StyleSheet,
    Text,
    TextInput,
    View
} from "react-native";
import Icon from "react-native-vector-icons/FontAwesome";
import { NavigationRoute, NavigationScreenProp } from "react-navigation";
import { getManageAgentPage, postManageAgentPage } from "../../../Api/ProfileAPI";
import AtrastiButton from "../../../Shared/AtrastiButton";
import CityPicker from "../../../Shared/CityPicker";
import CountryPicker from "../../../Shared/CountryPicker";
import CountryPickerEvent from "../../../utils/Events/Picker/CountryPickerEvent";
import StatePicker from "../../../Shared/StatePicker";
import { AgentPage_Res } from "../../../Api/Models/Responds/AgentPage_Res";
import { ManageAgent_Req } from "../../../Api/Models/Requests/ManageAgent_Req";
import ImagePicker, { Image as CroppedImage } from "react-native-image-crop-picker";

export interface ManageAgentViewProps {
    navigation: NavigationScreenProp<any, any>;
    route: NavigationRoute;
}

export interface ManageAgentPageState extends AgentPage_Res {
    loading: boolean;
    edited: boolean;
    image: CroppedImage | null,
}

export default class ManageAgentView extends React.Component<ManageAgentViewProps, ManageAgentPageState> {

    constructor(props: ManageAgentViewProps) {
        super(props);

        this.state = {
            loading: true,
            edited: false,
            address: '',
            city: '',
            county: '',
            country: '',
            description: '',
            website: '',
            phoneNumber: '',
            businessSector: '',
            mainProducts: '',
            mainMarkets: '',
            certificates: '',
            yearsOfExperience: '',
            logo: null,
            image: null
        }
    }

    componentDidMount() {
        // @ts-ignore
        this.props.navigation.setOptions({
            headerLeft: () => (
                <Icon name={'chevron-left'} color={'#FFFFFF'} onPress={() => this.props.navigation.goBack()} style={{
                    marginLeft: 10
                }}/>
            )
        });

        getManageAgentPage()
            .then(result => {
                const newState: ManageAgentPageState = {
                    ...result,
                    loading: false,
                    edited: false,
                    image: null
                };

                this.setState(newState);
            })
            .catch(errors => {
                console.log(errors);
                this.setState({
                    loading: false
                })
            })
    }

    render() {
        return (
            <View style={{
                backgroundColor: '#000000',
                flex: 1
            }}>
                {this.state.loading ? (
                    <ActivityIndicator size={"large"}/>
                ) : (
                    <ScrollView style={{
                        paddingLeft: 15,
                        paddingRight: 15
                    }}>
                        {this.state.image === null ? null :  (
                            <Image source={{
                                uri: this.state.image.path
                            }} style={{
                                width: 180,
                                height: 180,
                                alignSelf: "center"
                            }}/>
                        )}
                        <AtrastiButton title={'Upload picture'} onClick={() => {
                            this.onUploadPicture();
                        }} style={Styles.saveButton} textStyle={Styles.saveButtonText}/>

                        <Text style={Styles.header}>Personal</Text>
                        <Text style={Styles.label}>Address</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Address'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.address}
                        />
                        <Text style={Styles.label}>Phone number</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Phone number'}
                            onChangeText={this.onPhoneNumberChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.phoneNumber}
                        />
                        <Text style={Styles.label}>Country</Text>
                        <CountryPicker country={this.state.country} onValueChange={(country) => {
                            if(this.state.country !== country) {
                                DeviceEventEmitter.emit(CountryPickerEvent.EVENT_TYPE, new CountryPickerEvent(country, null))
                                this.setState({
                                    country: country,
                                    county: '',
                                    city: ''
                                })
                            }
                        }}/>
                        <StatePicker country={this.state.country} state={this.state.county} onValueChange={(state) => {
                            if(this.state.county !== state) {
                                DeviceEventEmitter.emit(CountryPickerEvent.EVENT_TYPE, new CountryPickerEvent(this.state.country, state))
                                this.setState({
                                    county: state,
                                    city: ''
                                });
                            }
                        }}/>
                        <CityPicker country={this.state.country} state={this.state.county} city={this.state.city}
                                    onValueChange={(city) => {
                                        this.setState({
                                            city: city
                                        })
                                    }}/>
                        <Text style={Styles.label}>Website</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Website'}
                            onChangeText={this.onWebsiteChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.website}
                        />
                        <Text style={Styles.label}>Description</Text>
                        <TextInput
                            style={[Styles.input, {
                                borderRadius: 15,
                                textAlignVertical: "top"
                            }]}
                            placeholder={'Description'}
                            onChangeText={this.onDescriptionChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            multiline={true}
                            numberOfLines={5}
                            value={this.state.description}
                        />
                        <Text style={Styles.header}>Information</Text>
                        <Text style={Styles.label}>Business sector</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Business sector'}
                            onChangeText={this.onBusinessSectorChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.businessSector}
                        />
                        <Text style={Styles.label}>Main products</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Main products'}
                            onChangeText={this.onMainProductsChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.mainProducts}
                        />
                        <Text style={Styles.label}>Main markets</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Main markets'}
                            onChangeText={this.onMainMarketsChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.mainMarkets}
                        />
                        <Text style={Styles.label}>Years of experience</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Years of experience'}
                            onChangeText={this.onYearsOfExperienceChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.yearsOfExperience.toString()}
                        />
                        <Text style={Styles.label}>Certificates</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Certificates'}
                            onChangeText={this.onCertificatesChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.certificates}
                        />
                        <AtrastiButton title={'Save changes'} onClick={(event) => {
                            const request: ManageAgent_Req = {
                                ...this.state,
                                logo: this.state.image?.data as string
                            };
                            postManageAgentPage(request)
                                .then(result => {

                                })
                                .catch(error => {

                                });
                        }} style={Styles.saveButton} textStyle={Styles.saveButtonText}/>
                    </ScrollView>
                )}
            </View>
        );
    }

    onUploadPicture() {
        ImagePicker.openPicker({
            width: 180,
            height: 180,
            cropping: true,
            includeBase64: true
        }).then(image => {
            this.setState({
                image: image
            });
        }).catch(error => {

        });
    }

    onAddressChange(data: string) {
        this.setState({
            address: data
        })
    }

    onPhoneNumberChange(data: string) {
        this.setState({
            phoneNumber: data
        })
    }

    onWebsiteChange(data: string) {
        this.setState({
            website: data
        })
    }

    onDescriptionChange(data: string) {
        this.setState({
            description: data
        })
    }

    onBusinessSectorChange(data: string) {
        this.setState({
            businessSector: data
        })
    }

    onMainProductsChange(data: string) {
        this.setState({
            mainProducts: data
        })
    }

    onMainMarketsChange(data: string) {
        this.setState({
            mainMarkets: data
        })
    }

    onYearsOfExperienceChange(data: string) {
        this.setState({
            yearsOfExperience: data
        })
    }

    onCertificatesChange(data: string) {
        this.setState({
            certificates: data
        })
    }
}

const Styles = StyleSheet.create({
    input: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        borderRadius: 50,
        color: '#FFFFFF',
        padding: 10
    },
    header: {
        fontWeight: "bold",
        marginTop: 10,
        fontSize: 22,
        color: '#FFFFFF'
    },
    label: {
        fontWeight: "bold",
        marginTop: 10,
        fontSize: 14,
        color: '#FFFFFF'
    },
    saveButton: {
        backgroundColor: '#29B6F6',
        marginTop: 10,
        borderRadius: 50,
        marginBottom: 10
    },
    saveButtonText: {
        textAlign: "center",
        color: "#FFFFFF",
        padding: 15
    }
})
