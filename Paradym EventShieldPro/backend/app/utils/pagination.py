class _PaginationResult:
    __slots__ = ('items', 'total', 'page', 'per_page', 'pages', 'has_next', 'has_prev')

    def __init__(self, **kwargs):
        for k, v in kwargs.items():
            setattr(self, k, v)


def paginate_results(query, page, per_page, serializer=None):
    pagination = query.paginate(page=page, per_page=per_page, error_out=False)
    items = [serializer(item) if serializer else item for item in pagination.items]
    return _PaginationResult(
        items=items,
        total=pagination.total,
        page=pagination.page,
        per_page=pagination.per_page,
        pages=pagination.pages,
        has_next=pagination.has_next,
        has_prev=pagination.has_prev,
    )
