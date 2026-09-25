"""
Report: articles created in the past N months, authored by users created in the past M
months, filtered by content language.

Usage:
    python article_report.py
(edit the variables below, or wire them up to argv/env as needed)
"""
import sqlite3

# ---- variables ----
DB_PATH = r"C:\Users\Vignesh.Haldankar\Downloads\Learning\CRUD_App\CRUD_App\crud_app.db"
ARTICLE_MONTHS = 3
USER_MONTHS = 4
LANGUAGE = "English"
# --------------------

QUERY = """
SELECT DISTINCT
    a.Id AS ArticleId,
    a.Status,
    a.CreatedAt AS ArticleCreatedAt,
    u.Id AS AuthorId,
    u.Username,
    u.CreatedAt AS AuthorCreatedAt,
    c.Language
FROM Articles a
JOIN Contents c ON c.ArticleId = a.Id
JOIN Users u ON u.Id = c.AuthorId
WHERE a.CreatedAt >= datetime('now', ?)
  AND u.CreatedAt >= datetime('now', ?)
  AND c.Language = ?;
"""


def main():
    conn = sqlite3.connect(DB_PATH)
    cur = conn.cursor()
    cur.execute(QUERY, (f"-{ARTICLE_MONTHS} months", f"-{USER_MONTHS} months", LANGUAGE))
    rows = cur.fetchall()
    columns = [d[0] for d in cur.description]

    print(f"Articles from the last {ARTICLE_MONTHS} month(s), by users created in the "
          f"last {USER_MONTHS} month(s), language={LANGUAGE}\n")
    print(" | ".join(columns))
    for row in rows:
        print(" | ".join(str(v) for v in row))

    conn.close()


if __name__ == "__main__":
    main()
