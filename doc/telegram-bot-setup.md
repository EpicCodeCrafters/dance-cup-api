# Telegram Bot Setup

## Overview

The Dance Cup API now supports Telegram notifications for tournament events. The bot sends notifications when tournament registration state changes.

## Configuration

To enable Telegram notifications, configure the `TgApiOptions` section in your `appsettings.json` or environment variables:

```json
{
  "TgApiOptions": {
    "BotToken": "YOUR_BOT_TOKEN_HERE",
    "ChatId": 123456789
  }
}
```

### Getting Bot Token

1. Talk to [@BotFather](https://t.me/botfather) on Telegram
2. Create a new bot using `/newbot` command
3. Copy the bot token provided by BotFather

### Getting Chat ID

1. Add your bot to the desired channel or group
2. Send a message to the bot or channel
3. Visit `https://api.telegram.org/bot<YOUR_BOT_TOKEN>/getUpdates`
4. Look for the `chat.id` field in the response

## Notifications

The bot sends notifications for the following events:

### 1. Registration Started
When tournament registration begins:
```
🎉 Регистрация на турнир "<Tournament Name>" началась!

📅 Дата турнира: <Tournament Date>
📝 Описание: <Tournament Description>
```

### 2. Registration Finished
When tournament registration ends:
```
✅ Регистрация на турнир "<Tournament Name>" завершена!

👥 Зарегистрировано пар: <Couples Count>
📋 Категорий: <Categories Count>
```

### 3. Registration Reopened
When tournament registration is reopened:
```
🔄 Регистрация на турнир "<Tournament Name>" возобновлена!

📅 Дата турнира: <Tournament Date>
👥 Уже зарегистрировано пар: <Couples Count>
```

## Notes

- If the bot token is not configured, the application will log warnings but continue to function normally
- All notification errors are logged but do not affect the core tournament functionality
- The bot gracefully handles missing configuration without throwing exceptions
