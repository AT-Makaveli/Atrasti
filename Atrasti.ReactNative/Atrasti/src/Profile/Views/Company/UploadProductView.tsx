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
import * as IonIcon from "react-native-vector-icons/Ionicons";
import * as FAIcon from "react-native-vector-icons/FontAwesome";
import { NavigationRoute, NavigationScreenProp } from "react-navigation";
import ImagePicker, { Image as CroppedImage } from 'react-native-image-crop-picker';
import AtrastiButton from "../../../Shared/AtrastiButton";
import { getUserCategories, uploadProduct } from "../../../Api/ProductAPI";
import { ProductCategories_Res } from "../../../Api/Models/Responds/ProductCategories_Res";
import { BaseCategory_Res } from "../../../Api/Models/Responds/BaseCategory_Res";
import CategoryPicker from "../../../Shared/CategoryPicker";

export interface UploadProductViewProps {
    navigation: NavigationScreenProp<any, any>;
    route: NavigationRoute;
}

export interface UploadProductPageState {
    image: CroppedImage | null,
    productTitle: string,
    productDescription: string,
    tags: string[],
    tagError: TagError[],
    loading: boolean,
    categories: BaseCategory_Res[],
    selectedCategory: number
}

interface TagError {
    index: number,
    error: string
}

export default class UploadProductView extends React.Component<UploadProductViewProps, UploadProductPageState> {

    constructor(props: UploadProductViewProps) {
        super(props);

        this.state = {
            image: null,
            productTitle: '',
            productDescription: '',
            tags: [''],
            tagError: [],
            loading: true,
            categories: [],
            selectedCategory: -1
        }
    }

    componentDidMount() {
        // @ts-ignore
        this.props.navigation.setOptions({
            headerLeft: () => (
                <FAIcon.default name={'chevron-left'} color={'#FFFFFF'} onPress={() => this.props.navigation.goBack()}
                                style={{
                                    marginLeft: 10
                                }}/>
            )
        });

        getUserCategories()
            .then(result => {
                this.setState({
                    categories: result.categories,
                    loading: false
                })
            })
            .catch(error => {
                console.log('UploadProductView');
                console.log(error);
            });
    }

    renderTags(): JSX.Element[] {
        return this.state.tags.map((tag, index) => {
            const isLastTag = index === (this.state.tags.length - 1);
            let tagErrorString = undefined;

            for (const tagError of this.state.tagError) {
                if(tagError.index === index) {
                    tagErrorString = tagError.error;
                }
            }

            return <View key={index}>
                <View style={{
                    flexDirection: "row",
                    marginTop: 10
                }}>
                    <TextInput
                        style={[Styles.input, {
                            flexGrow: 1,
                        }]}
                        value={this.state.tags[index]}
                        placeholder={this.state.tags.length === 1 ? 'Tag' : 'Tag ' + index}
                        placeholderTextColor={'#FEFEFE'}
                        onChange={(text) => {
                            this.onTextChange(text.nativeEvent.text, index);
                        }}
                    />
                    <IonIcon.default name={isLastTag ? 'add-outline' : 'remove-outline'}
                                     color={isLastTag ? '#29B6F6' : '#EF5350'} style={{
                        marginLeft: 10,
                        alignSelf: "center"
                    }} size={24} onPress={() => {
                        if(!isLastTag)
                            this.removeTag(index);
                        else
                            this.addTag();
                    }}/>
                </View>
                {tagErrorString !== undefined ? <Text style={{
                    color: '#EF5350',
                    paddingLeft: 10,
                    paddingTop: 4,
                    fontSize: 16
                }}>{tagErrorString}</Text> : null}
            </View>
        })
    }

    render() {
        if(this.state.loading) return this.renderLoading();

        return this.renderUploadProduct();
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

    renderUploadProduct() {
        return (
            <ScrollView style={{
                backgroundColor: '#000000',
                flex: 1,
                paddingLeft: 15,
                paddingRight: 15
            }}>
                {this.state.image === null ? null : (
                    <Image source={{
                        uri: this.state.image.path
                    }} style={{
                        width: 360,
                        height: 280,
                        alignSelf: "center"
                    }}/>
                )}
                <AtrastiButton title={'Upload picture'} onClick={() => {
                    this.onUploadPicture();
                }} style={Styles.button} textStyle={Styles.buttonText}/>
                <Text style={Styles.label}>Title</Text>
                <TextInput
                    style={Styles.input}
                    placeholder={'Product title'}
                    placeholderTextColor={'#FEFEFE'}
                    value={this.state.productTitle}
                    onChangeText={(text) => {
                        this.setState({
                            productTitle: text
                        })
                    }}
                />
                <Text style={Styles.label}>Description</Text>
                <TextInput
                    style={[Styles.input, {
                        borderRadius: 15,
                        textAlignVertical: "top"
                    }]}
                    placeholder={'Product description'}
                    placeholderTextColor={'#FEFEFE'}
                    value={this.state.productDescription}
                    multiline={true}
                    numberOfLines={4}
                    onChangeText={(text) => {
                        this.setState({
                            productDescription: text
                        })
                    }}
                />
                <Text style={Styles.label}>Tags</Text>
                {this.renderTags()}
                <Text style={Styles.label}>Category</Text>
                <CategoryPicker data={this.state.categories} selectedValue={this.state.selectedCategory}  onValueChange={(value) => {
                    this.setState({
                        selectedCategory: value
                    })
                }} />
                <AtrastiButton title={'Upload product'} onClick={() => {
                    this.onUploadProduct();
                }} style={Styles.uploadProductButton} textStyle={Styles.buttonText}/>
            </ScrollView>
        );
    }

    removeTag(index: number) {
        this.state.tags.splice(index, 1);
        this.setState({
            tags: this.state.tags
        })
    }

    addTag() {
        this.state.tags.push('');
        this.setState({
            tags: this.state.tags
        })
    }

    onUploadProduct() {
        let valid = this.validateTags();

        if(this.state.image?.data === null) {
            valid = false;
        }

        if (this.state.selectedCategory === -1) {
            valid = false;
        }

        if(valid) {
            const state = this.state;
            uploadProduct(state.image?.data as string, state.productTitle, state.productDescription, state.tags, this.state.selectedCategory)
                .then(result => {

                })
                .catch(errors => {

                })
        }
    }

    validateTags(): boolean {
        let tagErrors: Array<TagError> = [];
        for (let i = 0; i < this.state.tags.length; i++) {
            const tag = this.state.tags[i];
            const error = this.validateTag(tag);
            if(error != null) {
                tagErrors.push({
                    error: error,
                    index: i
                });
            }
        }

        if(tagErrors.length > 0) {
            this.setState({
                tagError: tagErrors
            });
            return false;
        }

        return true;
    }

    validateTag(tag: string) {
        if(tag.length === 0) return "Tag can't be empty"

        return null;
    }

    onTextChange(data: string, index: number) {
        this.state.tags[index] = data;
        this.setState({
            tags: this.state.tags
        })
    }

    onUploadPicture() {
        ImagePicker.openPicker({
            width: 360,
            height: 280,
            cropping: true,
            includeBase64: true
        }).then(image => {
            this.setState({
                image: image
            });
        }).catch(error => {

        });
    }
}

const Styles = StyleSheet.create({
    button: {
        backgroundColor: 'transparent',
        marginTop: 10,
        borderRadius: 50,
        borderColor: '#29B6F6',
        borderWidth: 2,
    },
    uploadProductButton: {
        backgroundColor: '#29B6F6',
        marginTop: 10,
        borderRadius: 50,
        borderColor: '#29B6F6',
        borderWidth: 2,
    },
    buttonText: {
        textAlign: "center",
        color: "#FFFFFF",
        padding: 10
    },
    label: {
        fontWeight: "bold",
        marginTop: 10,
        fontSize: 14,
        color: '#FFFFFF',
    },
    input: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        borderRadius: 50,
        color: '#FFFFFF',
        padding: 10,

    },
});
