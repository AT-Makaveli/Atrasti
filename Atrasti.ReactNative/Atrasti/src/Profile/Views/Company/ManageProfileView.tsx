import React from "react";
import {
    ActivityIndicator,
    Image,
    ScrollView,
    StyleSheet,
    Text,
    TextInput,
    View
} from "react-native";
import Icon from "react-native-vector-icons/FontAwesome";
import { NavigationRoute, NavigationScreenProp } from "react-navigation";
import { manageProfilePage, postManageCompanyPage } from "../../../Api/ProfileAPI";
import AtrastiButton from "../../../Shared/AtrastiButton";
import ImagePicker, { Image as CroppedImage } from "react-native-image-crop-picker";
import { mapSaveCompanyRequest } from "../../ManageProfileView";
import { BaseCategory_Res } from "../../../Api/Models/Responds/BaseCategory_Res";
import * as IonIcon from "react-native-vector-icons/Ionicons";
import CategoryPicker from "../../../Shared/CategoryPicker";
import CountryStateCityPicker from "../../../Shared/StateCityCountryPicker/CountryStateCityPicker";
import { loadCountries } from "../../../Api/CountryAPI";

export interface ManageProfileViewProps {
    navigation: NavigationScreenProp<any, any>;
    route: NavigationRoute;
}

export interface ManageProfilePageState {
    loading: boolean;
    edited: boolean;

    logo: string | null;
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
    yearEstablished: string;
    capacity: string;
    image: CroppedImage | null,
    allCategories: BaseCategory_Res[],
    selectedCategories: BaseCategory_Res[],
}

export default class ManageProfileView extends React.Component<ManageProfileViewProps, ManageProfilePageState> {

    private _country: string = '';
    private _county: string = '';
    private _city: string = '';

    constructor(props: ManageProfileViewProps) {
        super(props);

        this.state = {
            loading: true,
            edited: false,

            logo: null,
            image: null,
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
            yearEstablished: '2022',
            capacity: '',
            allCategories: [],
            selectedCategories: []
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

        Promise.all([loadCountries(), manageProfilePage()])
            .then(results => {
                const [, profile] = results;
                const newState: ManageProfilePageState = {
                    ...profile.company,
                    ...profile.companyInfo,
                    loading: false,
                    edited: false,
                    logo: this.state.image?.data as string,
                    image: null,
                    allCategories: profile.allCategories,
                    selectedCategories: profile.usedCategories
                };

                if (newState.selectedCategories.length === 0 && newState.allCategories.length > 0) {
                    newState.selectedCategories.push(newState.allCategories[0]);
                }

                this._country = profile.company?.country ?? '';
                this._county = profile.company?.county ?? '';
                this._city = profile.company?.city ?? '';

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
                        <Text style={Styles.label}>Logo</Text>
                        {this.state.image === null ? null : (
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
                        <CountryStateCityPicker
                            country={this.state.country}
                            state={this.state.county}
                            city={this.state.city}
                            onValueChange={(country, state, city) => {
                                this._country = country;
                                this._county = state;
                                this._city = city;

                                console.log('on change');
                                console.log(this._country);
                                console.log(this._county);
                                console.log(this._city);
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
                            placeholder={'Company description'}
                            onChangeText={this.onDescriptionChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            multiline={true}
                            numberOfLines={5}
                            value={this.state.companyDesc}
                        />
                        <Text style={Styles.header}>Company information</Text>
                        <Text style={Styles.label}>Business area</Text>
                        {this.renderBusinessAreas()}
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
                        <Text style={Styles.label}>Year established</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Year established'}
                            onChangeText={this.onYearEstablishedChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.yearEstablished.toString()}
                        />
                        <Text style={Styles.label}>Certificates</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Certificates'}
                            onChangeText={this.onCertificatesChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.certificates}
                        />
                        <Text style={Styles.label}>Capacity</Text>
                        <TextInput
                            style={Styles.input}
                            placeholder={'Capacity'}
                            onChangeText={this.onCapacityChange.bind(this)}
                            placeholderTextColor={'#FEFEFE'}
                            value={this.state.capacity}
                        />
                        <AtrastiButton title={'Save changes'} onClick={() => {
                            const req = mapSaveCompanyRequest(this.state, this._country, this._county, this._city);
                            postManageCompanyPage(req)
                                .then(() => {
                                    this.props.navigation.goBack();
                                })
                                .catch(error => {
                                    console.log(error);
                                });

                        }} style={Styles.saveButton} textStyle={Styles.saveButtonText}/>
                    </ScrollView>
                )}
            </View>
        );
    }

    renderBusinessAreas(): JSX.Element[] {
        const newCategories: BaseCategory_Res[] = [];
        if(this.state.selectedCategories.length > 0) {
            for (const category of this.state.selectedCategories) {
                newCategories.push(category);
            }
        } else if(this.state.allCategories.length > 0) {
            this.state.selectedCategories[0] = this.state.allCategories[0];
            newCategories.push(this.state.allCategories[0]);
        }

        return newCategories.map((tag, index) => {
            const isLastTag = index === (newCategories.length - 1);

            return <View key={index}>
                <View style={{
                    flexDirection: "row",
                    marginTop: 10
                }}>
                    <CategoryPicker data={this.state.allCategories}
                                    selectedValue={this.state.selectedCategories[index].id}
                                    onValueChange={(value, newIndex) => {
                                        this.state.selectedCategories[index] = this.state.allCategories[newIndex];
                                        this.setState({
                                            selectedCategories: this.state.selectedCategories
                                        })
                                    }}/>
                    <IonIcon.default name={isLastTag ? 'add-outline' : 'remove-outline'}
                                     color={isLastTag ? '#29B6F6' : '#EF5350'} style={{
                        marginLeft: 10,
                        alignSelf: "center"
                    }} size={24} onPress={() => {
                        if(!isLastTag)
                            this.removeBusinessArea(index);
                        else
                            this.addBusinessArea();
                    }}/>
                </View>
            </View>
        })
    }

    removeBusinessArea(index: number) {
        this.state.selectedCategories.splice(index, 1);
        this.setState({
            selectedCategories: this.state.selectedCategories
        })
    }

    addBusinessArea() {
        this.state.selectedCategories.push(this.state.allCategories[0]);
        this.setState({
            selectedCategories: this.state.selectedCategories
        })
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
            companyDesc: data
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

    onYearEstablishedChange(data: string) {
        this.setState({
            yearEstablished: data.replace(/[^0-9]/g, '')
        })
    }

    onCertificatesChange(data: string) {
        this.setState({
            certificates: data
        })
    }

    onCapacityChange(data: string) {
        this.setState({
            capacity: data
        })
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
            console.log(error);
        });
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
