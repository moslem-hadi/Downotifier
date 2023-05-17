import { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';

import { jobApiCallActions, modalActions, loadingActions } from '../_store';
import { List, Empty } from '../components';

export { Home };

function Home() {
  const dispatch = useDispatch();
  const { jobApiCalls } = useSelector(x => x.jobApiCalls);

  const handlePageChange = async page => {
    await getList(page);
  };
  useEffect(() => {
    getList(1);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const getList = async (page = 1) => {
    dispatch(loadingActions.startLoading());
    await dispatch(jobApiCallActions.getAll({ page }));
    dispatch(loadingActions.stopLoading());
  };
  return (
    <div>
      <div className="row">
        <div className="col-12 mb-3 mb-lg-5">
          <div className="position-relative card table-nowrap table-card">
            <div className="card-header d-flex align-items-center justify-content-between">
              <h5 className="mb-0">Api Calls</h5>
              <button
                type="button"
                className="btn btn-outline-secondary"
                onClick={() => dispatch(modalActions.openModal())}
              >
                Add new
              </button>
            </div>
            {jobApiCalls?.items?.length > 0 && (
              <List list={jobApiCalls} handlePageChange={handlePageChange} />
            )}
            {(jobApiCalls?.items?.length ?? 0) == 0 && <Empty />}
          </div>
        </div>
      </div>

      {jobApiCalls?.error && (
        <div className="alert alert-danger text-center">
          {jobApiCalls.error.message}
        </div>
      )}
    </div>
  );
}
