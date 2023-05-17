import Pagination from 'react-js-pagination';
import { Delete, Eye } from '../_helpers/isons';
import { useDispatch } from 'react-redux';
import { Link } from 'react-router-dom';
import { jobApiCallActions, loadingActions } from '../_store';
function List({ list, handlePageChange }) {
  const dispatch = useDispatch();
  const deleteHandle = async id => {
    dispatch(loadingActions.startLoading());
    await dispatch(jobApiCallActions.deleteItem({ id }));
    dispatch(loadingActions.stopLoading());
  };

  return (
    <>
      <div className="table-responsive">
        <table className="table mb-0">
          <thead className="small text-uppercase bg-body text-muted">
            <tr>
              <th>ID</th>
              <th>Title</th>
              <th>Url</th>
              <th>Interval</th> 
              <th></th>
            </tr>
          </thead>
          <tbody>
            {list.items.map((item, i) => (
              <tr className="align-middle" key={i}>
                <td>{item.id}</td>
                <td> {item.title}</td>
                <td>{item.url}</td>
                <td> {item.monitoringInterval}</td>

                <td width={100}>
                  <span className="icon me-3">
                    <Link to={`/view/${item.id}`}>
                      <Eye />
                    </Link>
                  </span>
                  <span
                    className="text-danger icon cursor"
                    onClick={() => deleteHandle(item.id)}
                  >
                    <Delete />
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="card-footer d-flex align-items-center justify-content-between">
        <Pagination
          activePage={list.pageNumber}
          itemsCountPerPage={list.itemsPerPage}
          totalItemsCount={list.totalCount}
          pageRangeDisplayed={5}
          onChange={handlePageChange}
          itemClass="page-item"
          linkClass="page-link"
        />
        <div>
          {list.totalCount} Api{list.totalCount > 1 && 's'} in{' '}
          {list.totalPages} page
          {list.totalPages > 1 && 's'}
          &nbsp;
          <small className="text-muted">
            ({list.itemsPerPage} items per page)
          </small>
        </div>
      </div>
    </>
  );
}

export { List };
