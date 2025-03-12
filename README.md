# FastORM

FastORM 是一個簡單且易於擴展的 ORM 專案，旨在提供靈活的數據訪問解決方案。

## 模組概述

- **FastORM.Cache**: 暫未實作，預計用於快取層的處理。
- **FastORM.Dapper**: 計劃中的 Dapper 整合，尚未完成。
- **FastORM.EFCore**: 使用 EF Core 提供基本的資料存取功能，現階段僅支援 CRUD 操作。
- **FastORM.Repository**: 定義基本的 Repository 介面與相關基礎邏輯。
- **FastORM.Tests**: 單元測試專案，確保程式碼品質與正確性。

## 目前進展

專案目前完成了 EF Core 的基本資料存取功能，並準備好進行擴展與增強。

## 專案目錄結構

```
FastORM/
├── FastORM.Cache/      # 未啟用的快取功能模組
├── FastORM.Dapper/     # 尚未完成的 Dapper 整合模組
├── FastORM.EFCore/     # 目前的主要模組，基於 EF Core 實現資料存取
├── FastORM.Repository/ # 提供基礎資料存取邏輯和介面
├── FastORM.Tests/      # 單元測試模組，確保穩定性和可靠性
└── FastORM.sln
```

## 使用方式

### 1. 引入 FastORM.Repository

在應用程式中新增對 FastORM.Repository 的參考，並建立所需的資料庫模型類型。

### 2. 設定 FastORM.EFCore

引入 FastORM.EFCore 並設定資料庫上下文和連接字串。

### 3. 進行資料操作

使用以下方法進行資料的 CRUD 操作：
- `GetByIdAsync()`
- `GetAllAsync()`
- `AddAsync()`
- `UpdateAsync()`
- `DeleteAsync()`

## 未來計畫

- 增加 Dapper 支援以滿足輕量化資料存取需求。
- 完成 Cache 模組以提升效能和減少資料庫查詢負擔。
- 提高測試覆蓋率，確保模組的穩定性與效能。
