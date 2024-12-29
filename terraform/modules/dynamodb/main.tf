resource "aws_dynamodb_table" "main" {
  name         = var.table_name
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = var.partition_key
  range_key    = var.sorting_key

  lifecycle {
    prevent_destroy = false
  }

  point_in_time_recovery {
    enabled = var.enable_point_in_time_recovery
  }

  dynamic "ttl" {
    for_each = var.ttl != "" ? [1] : []
    content {
      attribute_name = var.ttl
      enabled        = true
    }
  }

  dynamic "attribute" {
    for_each = var.attributes
    content {
      name = attribute.value.name
      type = attribute.value.type
    }
  }

  dynamic "global_secondary_index" {
    for_each = var.global_secondary_indexes
    content {
      name               = global_secondary_index.value.name
      hash_key           = global_secondary_index.value.hash_key
      range_key          = global_secondary_index.value.range_key
      projection_type    = global_secondary_index.value.projection_type
      non_key_attributes = global_secondary_index.value.non_key_attributes
    }
  }
}

resource "aws_iam_policy" "policy_basic_access" {
  name   = "${var.table_name}-dynamo-basic-access-policy"
  policy = <<POLICY
{
  "Version": "2012-10-17",
  "Statement": [
    {
        "Effect": "Allow",
        "Action": [
            "dynamodb:PutItem",
            "dynamodb:DeleteItem",
            "dynamodb:GetItem",
            "dynamodb:UpdateItem",
            "dynamodb:DescribeTable",
            "dynamodb:Scan"
        ],
        "Resource": [
          "${aws_dynamodb_table.main.arn}",
          "${aws_dynamodb_table.main.arn}/index/*"
        ]
    }
  ]
}
POLICY
}