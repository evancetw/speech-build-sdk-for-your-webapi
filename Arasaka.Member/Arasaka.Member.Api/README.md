# 開發說明

使用 EntityFramework Core with SQLite。

## 準備工作

安裝 dotnet-ef

```cmd
dotnet tool install --global dotnet-ef
```

## 啟動專案

1. clone 專案
2. 還原 DB
    ```cmd
    cd Arasaka.Member.Api
    dotnet ef database update
    ```

## 開發 - 異動 DB schema

1. 建立新的 ef migration
    ```cmd
    cd Arasaka.Member.Api
    dotnet ef migrations add {migration 的名稱}
    # example: dotnet ef migrations add InitialCreate
    ```
2. 更新 DB
    ```cmd
    dotnet ef database update
    ```

## Spec

v1

```
stateDiagram-v2

    [*] --> Unverified: Register
    Unverified --> Verified: Verify
    Verified --> [*]: Remove
    
    Verified --> Banned: Ban
    Banned --> Verified: Recover
    Banned --> [*]: Remove
```

v2

