class PaginationWrap<T> {
  int pageNumber;
  int skipRows;
  int pageSize;
  int totalRecords;
  List<T> items;

  PaginationWrap(
      {required this.pageNumber,
      required this.skipRows,
      required this.pageSize,
      required this.totalRecords,
      required this.items});

  factory PaginationWrap.fromJson(Map<String, dynamic> json) => PaginationWrap(
      pageNumber: json['pageNumber'],
      skipRows: json['skipRows'],
      pageSize: json['pageSize'],
      totalRecords: json['totalRecords'],
      items: json['items'] as List<T>);
}
