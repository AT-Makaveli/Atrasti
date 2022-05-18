import React, { Ref } from "react";
import {
    FlatList,
    Image,
    ListRenderItemInfo,
    StyleSheet,
    Text,
    TextInput,
    TouchableHighlight,
    TouchableOpacity,
    View
} from "react-native";
import { map, filter, debounceTime, distinctUntilChanged, Subject } from "rxjs";
import { cacheSearch, getSearchHistory, search } from "../Api/SearchAPI";
import { Search_Res } from "../Api/Models/Responds/Search_Res";
import { SearchEntry_Res } from "../Api/Models/Responds/SearchEntry_Res";
import { getLogoImage } from "../Api/APIData";
import { NavigationScreenProp } from "react-navigation";

interface SearchState {
    searchQuery: string,
    searchAlternatives: AdvancedSearch[],
    results: Search_Res | null,
    cache: Search_Res
}

interface SearchProps {
    navigation: NavigationScreenProp<any, any>
}

interface AdvancedSearch {
    title: string,
    selected: boolean
}

export default class SearchView extends React.Component<SearchProps, SearchState> {

    private _inputRef: TextInput | null = null;
    private _searchSubject: Subject<string>;

    constructor(props: SearchProps) {
        super(props);

        this.state = {
            searchQuery: "",
            searchAlternatives: [
                {
                    title: "Agents",
                    selected: false
                },
                {
                    title: "Companies",
                    selected: false
                }],
            results: null,
            cache: {
                result: []
            }
        }

        //this._inputRef = React.createRef();
        this._searchSubject = new Subject<string>();
    }

    componentDidMount() {
        this._searchSubject.pipe(
            // get value
            map((event: any) => {
                return event;
            })

            // Time in milliseconds between key events
            , debounceTime(250)

            // If previous query is diffent from current
            , distinctUntilChanged()

            // subscription for response
        ).subscribe((text: string) => {
            if (text.length > 2) {
                search(text).then(result => {
                    this.setState({
                        results: result
                    });
                }).catch(ex => {

                });
            } else if (text.length === 0) {
                this.setState({
                    results: null
                });
            }
        });

        this.fetchCache();
    }

    fetchCache() {
        getSearchHistory()
            .then(result => {
                this.setState({
                    cache: result
                })
            })
            .catch(error => {

            })
    }

    renderAlternativeEntry(entry: ListRenderItemInfo<AdvancedSearch>) {
        return (
            <TouchableHighlight activeOpacity={0.6} onPress={() => {
                this.onClickAdvanced(entry);
            }}>
                <View style={{
                    paddingLeft: 10,
                    paddingTop: 5,
                    paddingBottom: 5,
                    paddingRight: 10,
                    backgroundColor: entry.item.selected ? '#6c6868' : '#444040',
                    margin: 5,
                    borderRadius: 10
                }}>
                    <Text style={{
                        color: 'white'
                    }}>{entry.item.title}</Text>
                </View>
            </TouchableHighlight>
        )
    }

    onClickAdvanced(item: ListRenderItemInfo<AdvancedSearch>) {
        this.state.searchAlternatives[item.index].selected = !this.state.searchAlternatives[item.index].selected;
        this.setState({
            searchAlternatives: this.state.searchAlternatives
        })
    }

    renderSearchEntry(searchEntry: ListRenderItemInfo<SearchEntry_Res>) {
        return (
            <TouchableOpacity key={searchEntry.index} style={{
                flexDirection: 'row',
                alignItems: 'center',
                padding: 5
            }} onPress={() => {
                this.opClickSearchEntry(searchEntry.item);
            }}>
                <Image source={{
                    uri: getLogoImage(searchEntry.item.logo)
                }} style={{
                    width: 50,
                    height: 50,
                    borderRadius: 50,
                    justifyContent: "flex-start",
                    backgroundColor: '#29B6F6'
                }}/>
                <Text style={[Styles.item, {
                    justifyContent: "center"
                }]}>{searchEntry.item.companyName}</Text>

            </TouchableOpacity>)
    }

    render() {
        return (
            <View>
                <View style={Styles.mainView}>
                    <TextInput
                        style={Styles.input}
                        placeholder={'Search'}
                        onChangeText={this.onSearchChange.bind(this)}
                        placeholderTextColor={'#FEFEFE'}
                        ref={(element) => this._inputRef = element}
                    />
                    {
                        this.state.results !== null ? (
                            <FlatList
                                data={this.state.results.result}
                                keyExtractor={(item: any, index: number) => index.toString()}
                                renderItem={this.renderSearchEntry.bind(this)}
                            />
                        ) : (
                            <FlatList
                                data={this.state.cache.result}
                                keyExtractor={(item: any, index: number) => index.toString()}
                                renderItem={this.renderSearchEntry.bind(this)}
                            />
                        )
                    }
                </View>
            </View>
        )
    }

    onSearchChange(text: string) {
        this._searchSubject.next(text);
    }

    opClickSearchEntry(searchEntry: SearchEntry_Res) {
        if(searchEntry.companyId !== null) {
            cacheSearch(searchEntry.companyId)
                .then(() => {
                    this.fetchCache();
                })
                .catch(error => {

                });
            this.props.navigation.navigate('OtherProfile', {
                userId: searchEntry.companyId.toString()
            });
        }
    }
}

const Styles = StyleSheet.create({
    mainView: {
        backgroundColor: '#000000',
        display: "flex",
        minHeight: "100%",
        flexDirection: "column"
    },
    input: {
        borderColor: '#FFFFFF',
        backgroundColor: '#343a40',
        color: '#FFFFFF',
        padding: 10
    },
    item: {
        padding: 10,
        fontSize: 18,
        height: 44,
        color: '#FFFFFF'
    }
});
