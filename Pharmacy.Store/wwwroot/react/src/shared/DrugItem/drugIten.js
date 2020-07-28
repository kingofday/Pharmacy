import React from 'react';
import { connect } from 'react-redux';
import Skeleton from '@material-ui/lab/Skeleton';
import { Link } from 'react-router-dom';
import { commaThousondSeperator } from './../../shared/utils';
import strings from './../../shared/constant';
import { AddToBasketAction } from './../../redux/actions/basketAction';

class LoadingDrug extends React.Component {
    render() {
        return (<figure className='comp-loading-drug'>
            <Skeleton variant='rect' className='main-img' animation="wave" />
            <figcaption className='text-wrapper'>
                <Skeleton className='name' variant='text' />
                <Skeleton className='price' variant='text' />
                <Skeleton className='btn-add' variant='rect' animation="wave" />
            </figcaption>
        </figure>);
    }
}

class DrugItem extends React.Component {
    _addToBasket(item) {
        this.props.addToBasket(item, 1);
    }
    render() {
        if (this.props.loading) {
            return <LoadingDrug />;
        }
        const { item } = this.props;
        return (
            <figure className='comp-drug-item'>
                <Link to={`/product/${item.drugId}`} className='d-flex justify-content-center'><img className='main-img' src={item.thumbnailImageUrl} alt={item.nameFa} /></Link>
                <figcaption className='text-center'>
                    <h5 className='name'>{item.nameFa}</h5>
                    <p className='price'>{commaThousondSeperator(item.price)} {strings.currency}</p>
                    <button className='btn-add' onClick={() => this._addToBasket(item)}>
                        {strings.addToCart}
                    </button>
                </figcaption>
            </figure>

        );
    }
}
const mapDispatchToProps = dispatch => ({
    addToBasket: (item, count) => dispatch(AddToBasketAction(item, count))
});


export default connect(null, mapDispatchToProps)(DrugItem);