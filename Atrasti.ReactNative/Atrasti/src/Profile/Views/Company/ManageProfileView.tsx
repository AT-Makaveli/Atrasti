import React from "react";
import { ActivityIndicator, DeviceEventEmitter, ScrollView, StyleSheet, Text, TextInput, View } from "react-native";
import Icon from "react-native-vector-icons/FontAwesome";
import { NavigationRoute, NavigationScreenProp } from "react-navigation";
import { manageProfilePage } from "../../../Api/ProfileAPI";
import AtrastiButton from "../../../Shared/AtrastiButton";
import CityPicker from "../../../Shared/CityPicker";
import CountryPicker from "../../../Shared/CountryPicker";
import CountryPickerEvent from "../../../utils/Events/Picker/CountryPickerEvent";
import StatePicker from "../../../Shared/StatePicker";

export interface ManageProfileViewProps {
    navigation: NavigationScreenProp<any, any>;
    route: NavigationRoute;
}

export interface ManageProfilePageState {
    loading: boolean;
    edited: boolean;

    address: string;
    city: string;
    county: string;
    country: string;
    website: string;
    companyDesc: string;
    phoneNumber: string;
    businessType: string;
    mainProducts: string;
    mainMarkets: string;
    certificates: string;
    yearEstablished: number;
    capacity: string;
}

export default class ManageProfileView extends React.Component<ManageProfileViewProps, ManageProfilePageState> {

    constructor(props: ManageProfileViewProps) {
        super(props);

        this.state = {
            loading: true,
            edited: false,

            address: '',
            city: '',
            county: '',
            country: '',
            website: '',
            companyDesc: '',
            phoneNumber: '',
            businessType: '',
            mainProducts: '',
            mainMarkets: '',
            certificates: '',
            yearEstablished: 2022,
            capacity: '',
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

        manageProfilePage()
            .then(result => {
                const newState: ManageProfilePageState = {
                    ...result.company,
                    ...result.companyInfo,
                    loading: false,
                    edited: false
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
                        <Text style={Styles.header}>Company</Text>
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
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.phoneNumber}
                        />
                        <Text style={Styles.label}>Country</Text>
                        <CountryPicker country={this.state.country} onValueChange={(country) => {
                            if (this.state.country !== country) {
                                DeviceEventEmitter.emit(CountryPickerEvent.EVENT_TYPE, new CountryPickerEvent(country, null))
                                this.setState({
                                    country: country,
                                    county: '',
                                    city: ''
                                })
                            }
                        }}/>
                        <StatePicker country={this.state.country} state={this.state.county} onValueChange={(state) => {
                            if (this.state.county !== state) {
                                DeviceEventEmitter.emit(CountryPickerEvent.EVENT_TYPE, new CountryPickerEvent(this.state.country, state))
                                this.setState({
                                    county: state,
                                    city: ''
                                });
                            }
                        }}/>
                        <CityPicker country={this.state.country} state={this.state.county} city={this.state.city} onValueChange={(city) => {
                            this.setState({
                                city: city
                            })
                        }}/>
                        <Text style={Styles.label}>Website</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Website'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.website}
                        />
                        <Text style={Styles.label}>Description</Text>
                        <TextInput
                            style={[Styles.input, {
                                borderRadius: 15,
                                textAlignVertical: "top"
                            }]}
                            placeholder={'Company description'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            multiline={true}
                            numberOfLines={5}
                            value={this.state.companyDesc}
                        />
                        <Text style={Styles.header}>Company information</Text>
                        <Text style={Styles.label}>Business type</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Business type'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.businessType}
                        />
                        <Text style={Styles.label}>Main products</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Main products'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.mainProducts}
                        />
                        <Text style={Styles.label}>Main markets</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Main markets'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.mainMarkets}
                        />
                        <Text style={Styles.label}>Year established</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Year established'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.yearEstablished.toString()}
                        />
                        <Text style={Styles.label}>Certificates</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Certificates'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.certificates}
                        />
                        <Text style={Styles.label}>Capacity</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Capacity'}
                            onChangeText={this.onAddressChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.capacity}
                        />
                        <AtrastiButton title={'Save changes'} onClick={(event) => {
                            console.log('save changes');
                        }} style={Styles.saveButton} textStyle={Styles.saveButtonText}/>
                    </ScrollView>
                )}
            </View>
        );
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
