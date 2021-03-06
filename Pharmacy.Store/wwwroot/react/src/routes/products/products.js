import React from 'react';
import { Container, Row, Col, Pagination, PageItem } from 'react-bootstrap';
import { connect } from 'react-redux';
import Skeleton from '@material-ui/lab/Skeleton';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';

import { ShowInitErrorAction } from '../../redux/actions/InitErrorAction';
import { enums } from '../../shared/constant';
import srvDrug from './../../service/srvDrug';
import DrugItem from './../../shared/DrugItem/drugIten';
import SearchName from './comps/searchName';
import PriceRange from './comps/priceRange';
import Categories from './../../shared/categories/categories';
import DrugsList from './comps/drugsList';
import { SetPageNumberAction, SetMaxAvailablePriceAction, SetCategoryIdAction } from './../../redux/actions/productsAction';

const calcBasePageNumber = (pageNumber) => pageNumber < 3 ? 1 : pageNumber - 2;

class Products extends React.Component {
    constructor(props) {
        super(props);
        let query = new URLSearchParams(this.props.location.search);
        let pageNumber = query.get('pageNumber');
        let categoryId = query.get('categoryId');
        this.state = {
            loading: true,
            maxPrice: 1,
            type: 0,
            items: [],
            basePageNumber: calcBasePageNumber(this.props.pageNumber),
            lastPageNumber: 1,
            categoryId: categoryId ? parseInt(categoryId) : null,
            pageNumber: pageNumber ? parseInt(pageNumber) : 1
        }
    }



    async _fetchDrugs() {

        let get = await srvDrug.get({
            pageNumber: this.state.pageNumber,
            type: this.state.type,
            name: this.props.name,
            minPrice: this.props.minPrice,
            maxPrice: this.props.maxPrice,
            categoryId: this.state.categoryId//this.props.categoryId//query.get('categoryId')
        });
        if (!get.success) {
            this.props.showInitError(this._fetchData.bind(this), get.message);
            return;
        }
        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });
        this.props.setMaxAvailablePrice(get.result.maxPrice);
        this.setState(p => ({
            ...p,
            loading: false,
            items: get.result.items,
            basePageNumber: calcBasePageNumber(this.props.pageNumber),
            lastPageNumber: get.result.lastPageNumber
        }));
    }

    async componentDidMount() {
        await this._fetchDrugs();
    }

    async componentDidUpdate(props) {
        if (props.name !== this.props.name ||
            props.minPrice !== this.props.minPrice ||
            props.maxPrice !== this.props.maxPrice)
            await this._fetchDrugs();

    }

    async _handleType(e) {
        let type = e.target.value
        this.setState(p => ({ ...p, type: type }), async () => { await this._fetchDrugs(); });

    }

    // _handlePaging(number) {
    //     this.props.setPageNumber(number);

    // }
    render() {
        const pn = this.state.pageNumber;
        const sorts = enums.drugFilterType;
        let pageNumbers = [];
        for (let i = 0; i < 5; i++)
            pageNumbers.push(this.state.basePageNumber + i);
        return (
            <div id='page-products'>
                <Container>
                    <Row id='first-row' className="mb-15">
                        <Col xs={12} sm={8} className='d-flex flex-column'>
                            <Row>
                                <Col xs={12} className="mb-15">
                                    <FormControl variant="outlined" className='card'>
                                        <Select
                                            className='w-100'
                                            labelId="demo-simple-select-label"
                                            id="demo-simple-select"
                                            value={this.state.type}
                                            onChange={this._handleType.bind(this)}>
                                            {Object.keys(sorts).map((k, idx) => <MenuItem key={idx} value={sorts[k].value}>{sorts[k].desc}</MenuItem>)}
                                        </Select>
                                    </FormControl>

                                </Col>
                                <Col xs={12} className='mb-15'>
                                    <Row>
                                        {this.state.loading ? [0, 1, 2, 3, 4, 5, 6, 7, 8].map((x, idx) => (<Col key={idx} xs={12} sm={4}><DrugItem loading={true} /></Col>)) :
                                            this.state.items.map((item, idx) => <Col key={idx} xs={12} sm={4} className='justify-content-center'><DrugItem item={item} /></Col>)
                                        }
                                    </Row>

                                </Col>
                                <Col xs={12} sm={12} className='justify-content-center '>
                                    <Pagination className="justify-content-center  mb-15">
                                        <Pagination.First href={`/products?pageNumber=1` + (this.state.categorId ? `&categroId=${this.state.categorId}` : '')} />
                                        <Pagination.Prev disabled={pn === 1} href={`/products?pageNumber=${pn - 1}` + (this.state.categorId ? `&categroId=${this.state.categorId}` : '')} />
                                        {pageNumbers.map((i) => <PageItem active={pn === i} key={i}
                                            disabled={i > this.state.lastPageNumber}
                                            href={`/products?pageNumber=${i}` + (this.state.categorId ? `&categroId=${this.state.categorId}` : '')}>{i}</PageItem>)}
                                        <Pagination.Next disabled={pn >= this.state.lastPageNumber} href={`/products?pageNumber=${pn + 1}` + (this.state.categorId ? `&categroId=${this.state.categorId}` : '')} />
                                        <Pagination.Last href={`/products?pageNumber=${this.state.lastPageNumber}` + (this.state.categorId ? `&categroId=${this.state.categorId}` : '')} />
                                    </Pagination>
                                </Col>
                            </Row>

                        </Col>
                        <Col sm={4} className='d-none d-sm-block'>
                            <Row>
                                <Col xs={12}>
                                    <SearchName />
                                </Col>
                                <Col xs={12}>
                                    <PriceRange maxPrice={0} />
                                </Col>
                                <Col xs={12}>
                                    <Categories bordered={true} />
                                </Col>
                                <Col xs={12}>
                                    <DrugsList />
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                </Container>
            </div>
        );
    }
}

const mapStateToProps = state => {
    return { ...state.productsReducer };
}

const mapDispatchToProps = dispatch => ({
    showInitError: (fetchData, message) => dispatch(ShowInitErrorAction(fetchData, message)),
    setPageNumber: (pageNumber) => dispatch(SetPageNumberAction(pageNumber)),
    setCategoryId: (id) => dispatch(SetCategoryIdAction(id)),
    setMaxAvailablePrice: (max) => dispatch(SetMaxAvailablePriceAction(max))
});

export default connect(mapStateToProps, mapDispatchToProps)(Products);