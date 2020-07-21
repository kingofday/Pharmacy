import React from 'react';
import ReactDOM from 'react-dom';
import { connect } from 'react-redux';
import strings from './../constant';
import ThreeDotLoader from './../threeDotLoader/threeDotLoader';
import srvDrug from './../../service/srvDrug';
import { Row, Col } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { toast } from 'react-toastify';
import { AddToBasketAction } from './../../redux/actions/basketAction';

class SearchItem extends React.Component {
    _changeActiveIndex() {
        console.log(this.props.idx);
        this.props.changeActiveIndex(this.props.idx);
    }
    render() {
        const item = this.props.drug;
        return (
            <li className='drug-item' onMouseEnter={this._changeActiveIndex.bind(this)}>
                <Link to={`/product/${item.drugId}`}>
                    <Row>
                        <Col className='drug-info' xs={8} sm={8}>
                            <img src={item.thumbnailImageUrl} alt={item.nameFa} />
                            <Row>
                                <Col xs={12} sm={12}>{item.nameFa}</Col>
                                <Col xs={12} sm={12}>({strings.identifier}:{item.uniqueId})</Col>
                                <Col xs={12} sm={12}>{item.nameFa}</Col>
                            </Row>
                        </Col>
                        <Col className='price-wrapper' xs={4} sm={4}>
                            <strong>{item.price} {strings.currency}</strong>
                        </Col>
                    </Row>
                </Link>

            </li>
        );
    }
}
class ActiveItem extends React.Component {
    _addToBasket() {
        this.props.addToBasket(this.props.drug, 1);
    }
    render() {
        var item = this.props.drug;
        return (
            <div className='active-item'>
                <img src={item.thumbnailImageUrl} alt={item.nameFa} />
                <hr />
                <h4>{item.nameFa}</h4>
                <div><small>{item.uniqueId}</small></div>
                <div><big>{item.price} {strings.currency}</big></div>
                <hr />
                <p>{item.shortDescription}</p>
                <hr />
                <div className='count'><span>{item.count} {item.unitName} </span></div>
                <div className='btn-add-to-basket'>
                    <button type='button' onClick={() => this._addToBasket()}>{strings.addToCart}</button>
                </div>
            </div>
        );
    }
}
class SearchBar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            items: [],
            activeIndex: 0,
            loading: false,
            showResult: false
        };
    }
    componentDidMount() {
        document.addEventListener('mousedown', this.handleClickOutside.bind(this));
    }

    componentWillUnmount() {
        document.removeEventListener('mousedown', this.handleClickOutside.bind(this));
    }
    handleClickOutside(event) {
        if (this.wrapper && !this.wrapper.contains(event.target)) {
            this.setState(p => ({ ...p, activeIndex: 0, showResult: false, items: [] }));
        }
    }
    _addToBasket() {
        this.props.addToBasket(this.state.items[this.state.activeIndex]);
    }

    async _fetchData(e) {
        if (!e.target.value) {
            this.setState(p => ({ ...p, showResult: false, activeIndex: 0, items: [] }));
            return;
        }
        this.setState(p => ({ ...p, loading: true }));
        let search = await srvDrug.search(e.target.value);
        this.setState(p => ({ ...p, loading: false }));
        if (!search.success) {
            toast(search.message, { type: toast.TYPE.ERROR });
            return;
        }
        this.setState(p => ({ ...p, activeIndex: 0, showResult: true, items: search.result }));
    }
    _changeActiveIndex(idx) {
        this.setState(p => ({ ...p, activeIndex: idx }));
    }
    render() {
        return (
            <div id='comp-search-drug' ref={c => this.wrapper = c}>
                <div className='input-group'>
                    <input placeholder={strings.searchHere} onInput={this._fetchData.bind(this)} type='search' name='q' />
                    <i className='zmdi zmdi-search'></i>
                    <ThreeDotLoader loading={this.state.loading} />
                </div>
                <div className={this.state.showResult ? 'search-result' : 'search-result d-none'}>
                    {(this.state.showResult && this.state.items.length > 0) ? (<Row>
                        <Col xs={12} sm={12} lg={12} xl={7}>
                            <ul>
                                {
                                    this.state.items.map((item, idx) => <SearchItem key={idx} idx={idx} drug={item} changeActiveIndex={this._changeActiveIndex.bind(this)} />)
                                }
                            </ul>
                        </Col>
                        <Col className='active-item-wrapper d-none d-xl-flex' xl={5}>
                            {this.state.showResult ? <ActiveItem drug={this.state.items[this.state.activeIndex]}
                                addToBasket={() => this._addToBasket()} /> : null}
                        </Col>
                    </Row>) :
                        <p className='alert alert-warning'>{strings.thereIsNoResult}</p>}
                </div>
            </div>
        )
    }
}
// const mapStateToProps = state => {
//     return { ...state.basketReducer, ...state.authenticationReducer };
// }

const mapDispatchToProps = dispatch => ({
    addToBasket: (item) => dispatch(AddToBasketAction(item))
});

export default connect(null, mapDispatchToProps)(SearchBar);